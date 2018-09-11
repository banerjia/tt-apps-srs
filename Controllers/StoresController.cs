using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;

namespace tt_apps_srs.Controllers
{
    public class StoresController : Controller
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly IESIndex _esStoreClient;
        private int _client_id;
        private string _client_url_code;
        private string _client_name;
        private readonly IClientProvider _client;

        public StoresController(tt_apps_srs_db_context db, 
                                IClientProvider client,
                                ElasticClient esSvcClient)
        {
            _db = db;
            _client_id = client.ClientId;
            _client_name = client.Name;
            _client_url_code = client.UrlCode;
            _client = client;

            _esStoreClient = new ESIndex_Store(esSvcClient);

        }

        public async Task<IActionResult> Index(string q = null, string retailer = null, string state = null, ushort page = 1, ushort number_of_stores_per_page = 10)
        {
            #region Search Request Setup
            var searchConfig = new SearchRequest<ESIndex_Store_Document>{                 
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
            #endregion


            #region Search Criteria Setup
            List<QueryContainer> qryCriteria_Should = new List<QueryContainer>{
                new MatchQuery
                {
                    Field = "client.urlCode.keyword",
                    Query = _client.UrlCode
                }
            };

            List<QueryContainer> qryCriteria_Must = new List<QueryContainer>{
                new MatchAllQuery()
            };

            if (!String.IsNullOrEmpty(state))
                qryCriteria_Must.Add(
                    new MatchQuery
                    {
                        Field = "state.keyword",
                        Query = state
                    });
            if (!String.IsNullOrEmpty(retailer))
                qryCriteria_Must.Add(
                    new MatchQuery{ 
                        Field = "retailer.id.keyword", 
                    Query = retailer
                });


            if (!String.IsNullOrEmpty(q))
                qryCriteria_Must.Add(
                    new QueryStringQuery{
                            Query = q
                        }
                );    

            searchConfig.Query = new BoolQuery{
                Must = qryCriteria_Must,
                Should = qryCriteria_Should                
            };
            #endregion

            #region Send Search Request
            var resultObject = await _esStoreClient.SearchAsync<ESIndex_Store_Document>(searchConfig) ;

            var stores = resultObject.Documents;
            var agg_retailers = resultObject
                                .Aggregations
                                .Terms("Retailers")
                                .Buckets;
            var agg_states = resultObject
                                .Aggregations
                                .Terms("States")
                                .Buckets;
            #endregion

            ViewData["Title"] = "Stores";
            ViewData["agg_retailers"] = agg_retailers;
            ViewData["agg_states"] = agg_states;
            ViewData["hits"] = resultObject.Total;

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
                Active = store.Active,
                Id = id,
                Name = store.Name,
                Address = store.Address,
                Phone = store.Phone,
                RetailerId = store.RetailerId,
                RetailerName = store.Retailer.Name
            };
            ClientStore cr = store.ClientStores.FirstOrDefault();
            try
            {
                model.MaxOrderAmount = Convert.ToDecimal(cr.Properties.Object["MaxOrderAmount"]);
            }
            catch
            {
                model.MaxOrderAmount = null;
            }
            try
            {
                model.LocationNumber = Convert.ToInt32(cr.Properties.Object["LocationNumber"]);
            }
            catch
            {
                model.LocationNumber = model.LocationNumber;
            }

            model.Orders = await _db.ClientStoreOrders
                                        .Where( q => q.ClientStoreId == cr.Id)
                                        .OrderByDescending( o => o.CreatedAt)
                                        .Take(5)
                                        .ToListAsync();

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
            if(!ModelState.IsValid)
            {
                model.ClientRetailers = await _db.ClientRetailers
                                                    .Where(q => q.ClientId == _client_id)
                                                    .Select(s => s.Retailer)
                                                    .ToListAsync();

                return View("New", model);
            }
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
                clientStore.Store.RetailerId = model.RetailerId??(new Guid());
            }
            Dictionary<string, object> props = new Dictionary<string, object>();
            if(model.MaxOrderAmount != null)
                props.Add("MaxOrderAmount", model.MaxOrderAmount);

            if (model.LocationNumber != null)
                props.Add("LocationNumber", model.LocationNumber);

            if (props.Any())
                clientStore.Properties = new JsonObject<Dictionary<string, object>>
                {
                    Object = props
                };
            
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
                Addr_Ln_1 = store.Addr_Ln_1,
                Addr_Ln_2 = store.Addr_Ln_2,
                City = store.City,
                State = store.State,
                Zip = store.Zip,
                Phone = store.Phone           
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
            try
            {
                model.LocationNumber = Convert.ToInt32(storeClient.Properties.Object["LocationNumber"]);
            }
            catch
            {
                model.LocationNumber = null;
            }

            model.ClientRetailers = await _db.ClientRetailers
                                                .Where(q => q.ClientId == _client_id)
                                                .Select(s => s.Retailer)
                                                .ToListAsync();

            ViewData["Title"] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(store.Name.ToLower());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Store_AddEditModel model)
        {
            if(!ModelState.IsValid)
            {
                model.ClientRetailers = await _db.ClientRetailers
                                                    .Where(q => q.ClientId == _client_id)
                                                    .Select(s => s.Retailer)
                                                    .ToListAsync();
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
            storeToUpdate.Addr_Ln_1 = model.Addr_Ln_1;
            storeToUpdate.Addr_Ln_2 = model.Addr_Ln_2;
            storeToUpdate.City = model.City;
            storeToUpdate.State = model.State;
            storeToUpdate.Zip = model.Zip;
            storeToUpdate.Phone = model.Phone;

            if (!String.IsNullOrEmpty(model.NewRetailer.Name))
            {
                Retailer newRetailer = new Retailer
                {
                    Name = model.NewRetailer.Name,
                    ClientRetailers = new List<ClientRetailer> { new ClientRetailer { ClientId = _client_id } }
                };

                _db.Retailers.Add(newRetailer);
                storeToUpdate.Retailer = newRetailer;
            }
            else
                storeToUpdate.RetailerId = (Guid)model.RetailerId;
            
            ClientStore clientStore = storeToUpdate.ClientStores.FirstOrDefault(q => q.ClientId == _client_id);
            Dictionary<string, object> props = new Dictionary<string, object>();
            string[] keysToUpdate = new string[] { "MaxOrderAmount", "LocationNumber" };

            // Copy JSON data that is not relevant to this action as-is
            try
            {
                foreach(var key in clientStore.Properties.Object.Keys.Where( q => !keysToUpdate.Contains( q )))
                {
                    props.Add(key, clientStore.Properties.Object[key]);
                }
            }
            catch
            {
                props = new Dictionary<string, object>();
            }

            // Update JSON data that is relevant to this action
            // Only update the ones that are set to non-null values
            // This logic will drop the properties that don't have values
            foreach(string keyToUpdate in keysToUpdate)
            {
                try
                {
                    if (model.GetType().GetProperty(keyToUpdate).GetValue(model, null) != null)
                        props.Add(keyToUpdate, model.GetType().GetProperty(keyToUpdate).GetValue(model, null));
                }
                catch
                {
                    continue;
                }
                
            }
            // Finally associate the Properties to the
            // client-store combo
            clientStore.Properties = new JsonObject<Dictionary<string, object>>
            {
                Object = props
            };

            _db.Attach(clientStore).State = EntityState.Modified;
            

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
                model.ClientRetailers = await _db.ClientRetailers
                                                    .Where(q => q.ClientId == _client_id)
                                                    .Select(s => s.Retailer)
                                                    .ToListAsync();
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
            if (ESIndex)
                _esStoreClient.DeleteIndex();

            foreach (var storeToProcess in storesToProcess)
            {
                if(forceGeocode || storeToProcess.Latitude == 0 || storeToProcess.Latitude == null)
                {
                    GoogleGeocoding_Location location = GeneralPurpose.GetLatLong(storeToProcess.Address);
                    storeToProcess.Latitude = location.lat;
                    storeToProcess.Longitude = location.lng;
                    _db.Attach(storeToProcess).State = EntityState.Modified;
                }
                if(ESIndex)
                {
                    _esStoreClient.CreateAsAsync(storeToProcess);
                }
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
            _esStoreClient.CreateAsAsync(storeToIndex);
        }
        #endregion
    }

    #region Supporting View Models
    public class Store_DetailModel
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }

        public string Name { get; set; }
        public int? LocationNumber { get; set; }

        public bool Active { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public string RetailerName { get; set; }

        public decimal? MaxOrderAmount { get; set; }

        public ICollection<ClientStoreOrder> Orders {get;set;}
    }
    public class Store_AddEditModel 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? LocationNumber { get; set; }

        public string Addr_Ln_1 { get; set; }
        public string Addr_Ln_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string Phone { get; set; }
        public Guid? RetailerId { get; set; }


        [Range(0.0, (double)Decimal.MaxValue, ErrorMessage = "Please review the maximum order amount. Any amount of 0 or more is acceptable.")]
        public decimal? MaxOrderAmount { get; set; }
        public IEnumerable<Retailer> ClientRetailers { get; set; }
        public Retailer NewRetailer { get; set; }

        public string Address
        {
            get
            {
                string retval = Addr_Ln_1;
                if (!String.IsNullOrEmpty(Addr_Ln_2))
                    retval += ", " + Addr_Ln_2;
                retval += String.Format(", {0}, {1}", City, State);
                if (!String.IsNullOrEmpty(Zip))
                    retval += " - " + Zip;

                return retval.Trim();
            }
        }
    }
    #endregion
}