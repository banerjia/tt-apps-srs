using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;
using ZXing;
using ZXing.Common;

namespace tt_apps_srs.Controllers
{
    public class OrdersController: Controller
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly IClientProvider _client;

        public OrdersController(tt_apps_srs_db_context db, 
                                IClientProvider client)
        {
            _db = db;
            _client = client;

        }

        public async Task<IActionResult> Index()
        {
            var orders = await _db.ClientStoreOrders
                                    .Include( i => i.Items)
                                    .Include( i => i.ClientStore)
                                        .ThenInclude( ti => ti.Store)
                                            .ThenInclude( ti2 => ti2.Retailer)
                                    .OrderByDescending( o => o.CreatedBy)
                                    .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> New(Guid store)
        {
            Order_AddEditModel model = new Order_AddEditModel();

            if(store != null)
            {
                ClientStore cs = await _db.ClientStores
                                                .Include( i => i.Store)
                                                .FirstOrDefaultAsync( q => q.StoreId == store);
                model.ClientStoreId = cs.Id;
                model.Store_Name = cs.Store.Name;
                model.Store_Address = cs.Store.Address;
            }

            model.AvailableProducts = await _db.ClientRetailerProducts.ToListAsync();
            ViewData["Title"] = String.Format("{0}: New Order", _client.Name);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order_AddEditModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("New", model);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = model.Id});
        }

    }

    public class Order_AddEditModel
    {
        public Order_AddEditModel()
        {
            this.Items = new List<ClientStoreOrderProduct>();
            this.AvailableProducts = new List<ClientRetailerProduct>();
        }
        public Guid? Id { get; set; }

        public string Store_Name { get; set; }
        public string Store_Address { get; set; }

        public int ClientStoreId { get; set; }
        public string Notes { get; set; }

        public string Status { get; set; }

        public decimal? Total { get; set; }

        public ICollection<ClientRetailerProduct> AvailableProducts { get; set; }
        public ICollection<ClientStoreOrderProduct> Items { get; set; }

        public IDictionary<string, string> Statuses {
            get{
                return new Dictionary<string, string>{
                    {"actv", "Active"},
                    {"void", "Voided"}
                };
            }
        }

    }

    public class Order_AddEdit_ProductModel{

        public Order_AddEdit_ProductModel()
        {
            this.Status = "actv";
        }

        public int ClientRetailerProductId { get; set; }
        public string Product_Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public string Status { get; set; }
    }
}