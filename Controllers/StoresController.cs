using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tt_apps_srs.Models;

namespace tt_apps_srs.Controllers
{
    public class StoresController : Controller
    {
        private readonly tt_apps_srs_db_context _db;

        public StoresController(tt_apps_srs_db_context db)
        {
            _db = db;
        }

        public IActionResult Index(string client_url_code)
        {
            IEnumerable<Store> stores = _db.Stores.Where(q => q.Active && q.Retailer.ClientRetailer.Client.UrlCode == client_url_code);
            ViewData["client"] = client_url_code;
            ViewData["Title"] = "Stores";
            return View(stores);
        }

        public IActionResult New(string client_url_code)
        {
            Store_NewModel model = new Store_NewModel();

            model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.Client.UrlCode == client_url_code);

            if(!model.ClientRetailers.Any()){
                Retailer _tmp = new Retailer{ Active = true, Name = "Retailer 2"};
                _db.Retailers.Add(_tmp);

                _db.ClientRetailers.Add(new ClientRetailer{ ClientId = 1, Retailer = _tmp, Active = true});
                _db.SaveChanges();

                model.ClientRetailers = _db.Retailers.Where(q => q.Active && q.ClientRetailer.Client.UrlCode == client_url_code);
            }

            ViewData["Title"] = "Add New Store";
            ViewData["client"] = client_url_code;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string client_url_code, Store_NewModel model)
        {
            Store storeToSave = new Store{
                RetailerId = model.RetailerId,
                Name = model.Name,
                Active = true, 
                Addr_Ln_1 = model.Addr_Ln_1,
                Addr_Ln_2 = model.Addr_Ln_2,
                City = model.City,
                State = model.State,
                Country = "USA"
            };

            _db.Stores.Add(storeToSave);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(string client_url_code)
        {
            var store = _db.Stores.FirstOrDefault();
            ViewData["client"] = client_url_code;
            ViewData["Title"] = "Store: " + store.Name;
            return View(store);
        }
    }

    public class Store_NewModel : Store
    {
        public IEnumerable<Retailer> ClientRetailers { get; set; }
    }
}