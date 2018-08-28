﻿using System;
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

namespace tt_apps_srs.Controllers
{
    public class StoresController : Controller
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly ClientProvider _client;
        private int _client_id;
        private string _client_url_code;
        private string _client_name;
        private readonly I_ES_Index _es = new ES_Index_Store();

        public StoresController(tt_apps_srs_db_context db, 
                                IClientProvider client)
        {
            _db = db;
            _client_id = client.ClientId;
            _client_name = client.Name;
        }

        public IActionResult Index()
        {
            IEnumerable<Store> stores = _db.Stores
                                            .Include( i => i.Retailer)
                                            .Where(q => q.Active 
                                                && q.Retailer.ClientRetailer.ClientId == _client_id);
            ViewData["client"] = _client_name;
            ViewData["Title"] = "Stores";
            return View(stores);
        }

        public IActionResult New()
        {
            Store_NewModel model = new Store_NewModel();

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
        public async Task<IActionResult> Create(Store_NewModel model)
        {
            Store storeToSave = new Store{
                RetailerId = model.RetailerId,
                Name = model.Name,
                Active = true, 
                Addr_Ln_1 = model.Addr_Ln_1,
                Addr_Ln_2 = model.Addr_Ln_2,
                City = model.City,
                State = model.State,
                Country = "USA",
                
            };
            string address = String.Format("{0}, {1}, {2}-{3},US", storeToSave.Addr_Ln_1, storeToSave.City, storeToSave.State, storeToSave.Zip);
            GoogleGeocoding_Location location = GeneralPurpose.GetLatLong(address);
            storeToSave.Latitude = location.lat;
            storeToSave.Longitude = location.lng;

            storeToSave.ClientStores.Ad
                
                (new ClientStore
            {
                ClientId = _client_id,
                Properties = model.Properties
            });

            _db.Stores.Add(storeToSave);
            await _db.SaveChangesAsync();
            _es.CreateAsAsync(storeToSave);

            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid id)
        {
            var store = _db.Stores
                                .Include( i => i.Retailer)
                                .FirstOrDefault( q => q.Id == id);

            ViewData["client"] = _client_name;
            ViewData["Title"] = "Store: " + store.Name;

            return View(store);
        }
    }

    public class Store_NewModel : Store
    {
        public IEnumerable<Retailer> ClientRetailers { get; set; }
        public JsonObject Properties { get; set; }
    }
}