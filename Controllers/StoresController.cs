using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;

namespace tt_apps_srs.Controllers
{
    public class StoresController : Controller
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly ClientProvider _client;
        private int _client_id;
        private string _client_url_code;
        private string _client_name;
        private IESIndex _es;

        public StoresController(tt_apps_srs_db_context db, 
                                IClientProvider client)
        {
            _db = db;
            _client_id = client.ClientId;
            _client_name = client.Name;
            _client_url_code = client.UrlCode;

            _es = new ESIndex_Store();

        }

        public async Task<IActionResult> Index(string q = null, ushort page = 1, ushort number_of_stores_per_page = 10)
        {
            AggregationDictionary v = new AggregationDictionary();

            var searchConfig = new SearchRequest<ESIndex_Store_Document> {
                Size = number_of_stores_per_page,
                From = (page-1) * number_of_stores_per_page,
                Aggregations = new AggregationDictionary
                {
                    {
                        "Retailers",
                        new TermsAggregation("retailer.agg_name.keyword")
                        {
                            Field = "retailer.agg_Name.keyword",
                            Order = new List<TermsOrder>
                            {
                                new TermsOrder { Key = "_key", Order = SortOrder.Ascending}
                                 
                            }
                        }                           
                    },
                    {
                        "States",
                        new TermsAggregation("state.keyword")
                        {
                            Field = "state.keyword",
                            Order = new List<TermsOrder>
                            {
                                new TermsOrder { Key = "_key", Order = SortOrder.Ascending}

                            }
                        }
                    }
                }
            };

            if (!String.IsNullOrEmpty(q))
            {
                searchConfig.Query = new BoolQuery
                {
                    Must = new List<QueryContainer> 
                    {
                        new TermQuery 
                        {
                                Field = "clients.urlCode.keyword",
                                Value = _client_url_code.ToLower()
                        },
                        new QueryStringQuery{
                            DefaultField = "_all",
                            Query = q
                        }
                    }
                };
            }
            else
                searchConfig.Query = new BoolQuery { 
                                                Must = new List<QueryContainer> {
                                                            new TermQuery {
                                                                    Field = "clients.urlCode.keyword",
                                                                    Value = _client_url_code.ToLower()
                                                            }
                                                }
                };

                


            var resultObject = await _es.SearchAsync<ESIndex_Store_Document>(searchConfig) ;

            var stores = resultObject.Documents;
            var agg_retailers = resultObject
                                .Aggregations
                                .Terms("Retailers")
                                .Buckets;
            var agg_states = resultObject
                                .Aggregations
                                .Terms("States")
                                .Buckets;


            ViewData["Title"] = "Stores";
            ViewData["page"] = page;
            ViewData["agg_retailers"] = agg_retailers;
            ViewData["agg_states"] = agg_states;

            return View(stores);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var store = await _db.Stores
                                .Include(i => i.Retailer)
                                .Include(i => i.ClientStores)
                                .FirstOrDefaultAsync(q => q.Id == id );

            Store_DetailModel model = new Store_DetailModel
            {
                Id = id,
                Name = store.Name,
                LocationNumber = store.LocationNumber,
                Addr_Ln_1 = store.Addr_Ln_1,
                Addr_Ln_2 = store.Addr_Ln_2,
                City = store.City,
                State = store.State,
                Zip = store.Zip,
                Country = store.Country,
                Phone = store.Phone,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                MaxOrderAmount = Convert.ToDecimal(store.ClientStores.FirstOrDefault(q => q.ClientId == _client_id).Properties.Object["MaxOrderAmount"])
            };

            ViewData["Title"] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(store.Name.ToLower());

            return View(model);
        }

        public async Task<IActionResult> New()
        {
            Store_AddEditModel model = new Store_AddEditModel();

            model.ClientRetailers = await _db.ClientRetailers
                                            .Include(i => i.Retailer)
                                            .Where( q => q.Active  && q.ClientId == _client_id)
                                            .Select( s => s.Retailer)
                                            .ToListAsync();            
                 

            ViewData["Title"] = "Add New Store";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Store_AddEditModel model)
        {
            ClientStore clientStore = new ClientStore{
                ClientId = _client_id,
                Store = new Store{
                    Name = model.Name,
                    Active = true, 
                    Addr_Ln_1 = model.Addr_Ln_1,
                    Addr_Ln_2 = model.Addr_Ln_2,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    Country = "USA",
                    Phone = model.Phone
                },
                Active = true
            };

            if(!String.IsNullOrEmpty(model.NewRetailer.Name)){
                Retailer newRetailer = new Retailer {
                    Name = model.NewRetailer.Name,
                    ClientRetailers = new List<ClientRetailer> { new ClientRetailer { ClientId = _client_id } }
                };

                _db.Retailers.Add(newRetailer);
                clientStore.Store.Retailer = newRetailer;
            }
            else
            {
                clientStore.Store.RetailerId = model.RetailerId;
            }
            
            if(model.MaxOrderAmount != null)
            {
                clientStore.Properties = new JsonObject<Dictionary<string, object>>
                {
                    Object = new Dictionary<string, object>{
                        {"MaxOrderAmount", model.MaxOrderAmount }
                    }
                };
            }            
            
            GoogleGeocoding_Location location = GeneralPurpose.GetLatLong(clientStore.Store.Address);
            clientStore.Store.Latitude = location.lat;
            clientStore.Store.Longitude = location.lng;

            _db.ClientStores.Add(clientStore);
            await _db.SaveChangesAsync();

            IndexStoreWithES(clientStore.Store.Id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var store = await _db.Stores
                                 .Include( i => i.ClientStores)
                                 .FirstOrDefaultAsync( q => q.Id == id);

            if (store == null)
                return View("StoreNotFound");

            Store_AddEditModel model = new Store_AddEditModel {
                Id = id,
                Name = store.Name,
                LocationNumber = store.LocationNumber,
                Addr_Ln_1 = store.Addr_Ln_1,
                Addr_Ln_2 = store.Addr_Ln_2,
                City = store.City,
                State = store.State,
                Zip = store.Zip,
                Country = store.Country,
                Phone = store.Phone,
                Latitude = store.Latitude,
                Longitude = store.Longitude            
            };

            var storeClient = store.ClientStores.FirstOrDefault(q => q.ClientId == _client_id);
            try
            {
                model.MaxOrderAmount = Convert.ToDecimal(storeClient.Properties.Object["MaxOrderAmount"]);
            }
            catch
            {
                model.MaxOrderAmount = null;
            }

            //model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);

            ViewData["Title"] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(store.Name.ToLower());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Store_AddEditModel model)
        {
            if(!ModelState.IsValid)
            {
                //model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);
                View("Edit", model);
            }

            Store storeToUpdate = await _db.Stores
                                            .Include(i=> i.ClientStores)
                                            .FirstOrDefaultAsync(q => q.Id == model.Id);

            if (storeToUpdate == null)
                return NotFound();

            string addr_cmp_old = storeToUpdate.Address.ToUpper();
            string addr_cmp_new = model.Address.ToUpper();

            storeToUpdate.Name = model.Name;
            storeToUpdate.LocationNumber = model.LocationNumber;
            storeToUpdate.Addr_Ln_1 = model.Addr_Ln_1;
            storeToUpdate.Addr_Ln_2 = model.Addr_Ln_2;
            storeToUpdate.City = model.City;
            storeToUpdate.State = model.State;
            storeToUpdate.Zip = model.Zip;
            storeToUpdate.RetailerId = model.RetailerId;
            storeToUpdate.Phone = model.Phone;
            
            ClientStore clientStore = storeToUpdate.ClientStores.FirstOrDefault(q => q.ClientId == _client_id);
            Dictionary<string, object> props = new Dictionary<string, object>();
            try
            {
                foreach(var key in clientStore.Properties.Object.Keys.Where( q => q.ToUpper().Trim() != "MAXORDERAMOUNT"))
                {
                    props.Add(key, clientStore.Properties.Object[key]);
                }
            }
            catch
            {
                props = new Dictionary<string, object>();
            }

            if(model.MaxOrderAmount != null)
                props.Add("MaxOrderAmount", model.MaxOrderAmount);

            clientStore.Properties.Object = props;

            _db.Attach(clientStore).State = EntityState.Modified;

            if(!String.IsNullOrEmpty(model.NewRetailer.Name)){
                Retailer newRetailer = new Retailer{  
                    Name = model.NewRetailer.Name,
                    ClientRetailers = new List<ClientRetailer> { new ClientRetailer { ClientId = _client_id } }
                };

                _db.Retailers.Add(newRetailer);
                storeToUpdate.Retailer = newRetailer;
            }
            else
            {
                storeToUpdate.RetailerId = model.RetailerId;
            }
            

            // Only Geocode if the address has changed
            if(addr_cmp_new != addr_cmp_old 
                || storeToUpdate.Latitude == null 
                || storeToUpdate.Latitude == 0)
            {                
                GoogleGeocoding_Location location = GeneralPurpose.GetLatLong(addr_cmp_new);
                storeToUpdate.Latitude = location.lat;
                storeToUpdate.Longitude = location.lng;
            }

            _db.Attach(storeToUpdate).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {

                //model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);
                View("Edit", model);
            }

            // Reindex with ES
            IndexStoreWithES(storeToUpdate.Id);

            return RedirectToAction("Details", new { id = model.Id });
        }

        public async Task<ActionResult> ProcessStores(bool forceGeocode = false, bool ESIndex = true)
        {
            var storesToProcess = await _db.Stores
                                           .Include( s => s.ClientStores)
                                           .ThenInclude( cs => cs.Client)
                                           .Include( s => s.Retailer)
                                           .ToListAsync();
            foreach(var storeToProcess in storesToProcess)
            {
                if(forceGeocode || storeToProcess.Latitude == 0 || storeToProcess.Latitude == null)
                {
                    GoogleGeocoding_Location location = GeneralPurpose.GetLatLong(storeToProcess.Address);
                    storeToProcess.Latitude = location.lat;
                    storeToProcess.Longitude = location.lng;
                    _db.Attach(storeToProcess).State = EntityState.Modified;
                }
                if(ESIndex)
                    _es.CreateAsAsync(storeToProcess);
            }
            
            await _db.SaveChangesAsync();

            return Ok();
        }

        #region Private Supporting Functions
        private void IndexStoreWithES(Guid storeId)
        {
            var storeToIndex =  _db.Stores
                                           .Include( s => s.ClientStores)
                                           .ThenInclude( cs => cs.Client)
                                           .Include( s => s.Retailer)
                                           .FirstOrDefault( q => q.Id == storeId);
            _es.CreateAsAsync(storeToIndex);
        }
        #endregion
    }

    #region Supporting View Models
    public class Store_DetailModel : Store
    {
        public decimal? MaxOrderAmount { get; set; }
    }
    public class Store_AddEditModel : Store
    {
        [Range(0.0, (double)Decimal.MaxValue, ErrorMessage = "Please review the maximum order amount. Any amount of 0 or more is acceptable.")]
        public decimal? MaxOrderAmount { get; set; }
        public IEnumerable<Retailer> ClientRetailers { get; set; }
        public Retailer NewRetailer { get; set; }
    }
    #endregion
}