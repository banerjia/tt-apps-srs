using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tt_apps_srs.Models;
using tt_apps_srs.Lib;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nest;

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

            var searchConfig = new SearchRequest<ESIndex_Store_Document> {
                Query = new MatchAllQuery(),
                Size = number_of_stores_per_page,
                From = page * number_of_stores_per_page
            };


            var resultObject = await _es.SearchAsync<ESIndex_Store_Document>(searchConfig) ;

            var stores = resultObject.Documents;

            
            ViewData["client"] = _client_name;
            ViewData["Title"] = "Stores";
            ViewData["page"] = page;
            return View(stores);
        }

        public IActionResult Details(Guid id)
        {
            var store = _db.Stores
                                .Include(i => i.Retailer)
                                .FirstOrDefault(q => q.Id == id);

            ViewData["client"] = _client_name;
            ViewData["Title"] = "Store: " + store.Name;

            return View(store);
        }

        public async Task<IActionResult> New()
        {
            Store_AddEditModel model = new Store_AddEditModel();

            model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);

            if(!model.ClientRetailers.Any()){
                Retailer _tmp = new Retailer{ Active = true, Name = "Retailer 2"};
                _db.Retailers.Add(_tmp);

                _db.ClientRetailers.Add(new ClientRetailer{ ClientId = 1, Retailer = _tmp, Active = true});
                _db.SaveChanges();

                model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);
            }

            ViewData["client"] = _client_name;
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
                    RetailerId = model.RetailerId,
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
            var store = await _db.Stores.FirstOrDefaultAsync( q => q.Id == id);

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
            model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Store_AddEditModel model)
        {
            if(!ModelState.IsValid)
            {
                model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);
                View("Edit", model);
            }

            Store storeToUpdate = await _db.Stores.FindAsync(model.Id);

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

                model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.ClientId == _client_id);
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
    public class Store_AddEditModel : Store
    {
        public IEnumerable<Retailer> ClientRetailers { get; set; }
        public JsonObject Properties { get; set; }
    }
    #endregion
}