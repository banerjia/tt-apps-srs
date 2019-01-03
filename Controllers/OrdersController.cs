using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;

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

            ViewData["Title"] = String.Format("{0}: Orders", _client.Name);

            return View(orders);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var model = await _db.ClientStoreOrders
                                    .Include(i => i.ClientStore)
                                        .ThenInclude(ti => ti.Store)
                                    .Include(i => i.Items)
                                    .ThenInclude(ti => ti.ClientRetailerProduct)
                                    .FirstOrDefaultAsync(q => q.Id == id);
            ViewData["Title"] = String.Format("Order for {0}", model.ClientStore.Store.Name);
            return View(model);
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
            model.Items = new List<ClientStoreOrderProduct>
            {
                new ClientStoreOrderProduct{ ClientRetailerProductId = 1, Quantity = 1, UnitPrice = 1},
                new ClientStoreOrderProduct{ ClientRetailerProductId = 2, Quantity = 1, UnitPrice = 3},
                new ClientStoreOrderProduct{ ClientRetailerProductId = 3, Quantity = 4, UnitPrice = 2},
                new ClientStoreOrderProduct{ ClientRetailerProductId = 4, Quantity = 3, UnitPrice = 3}
            };
            ViewData["Title"] = String.Format("{0}: New Order", _client.Name);
            return View(model);
        }

        public async Task<IActionResult> OrderItem(int id, int Quantity = 1,int index=0)
        {
            var model = await _db.ClientRetailerProducts.FindAsync(id);
            ViewData["qty"] = Quantity;
            ViewData["id"] = index;
            return PartialView("_OrderItem", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order_AddEditModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("New", model);
            }

            await _db.ClientStoreOrders.AddAsync(new ClientStoreOrder
            {
                Id = Guid.NewGuid(),
                ClientStoreId = model.ClientStoreId,
                Total = model.Items.Where(q => q.Quantity > 0).Sum(s => s.Quantity * s.UnitPrice),
                Items = model
                            .Items
                                .Where( q => q.Quantity > 0)
                                .Select(s => new ClientStoreOrderProduct
                                    {
                                        ClientRetailerProductId = s.ClientRetailerProductId,
                                        Quantity = s.Quantity,
                                        UnitPrice = s.UnitPrice,
                                        Status = "actv"
                                    }).ToList(),
                Status = "actv",
                CreatedAt = DateTime.Now
            });

            

            await _db.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = model.Id});
        }

    }

    public class Order_AddEditModel
    {
        public Order_AddEditModel()
        {
        }
        public Guid? Id { get; set; }

        public string Store_Name { get; set; }
        public string Store_Address { get; set; }

        public int ClientStoreId { get; set; }
        public string Notes { get; set; }

        public string Status { get; set; }

        public decimal? Total { get; set; }

        public IList<ClientRetailerProduct> AvailableProducts { get; set; }
        public IList<ClientStoreOrderProduct> Items { get; set; }

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