using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;

namespace tt_apps_srs.Controllers
{
    public class ProductsController: Controller
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly IClientProvider _client;

        public ProductsController(tt_apps_srs_db_context db, 
                                IClientProvider client)
        {
            _db = db;
            _client = client;

        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.ClientRetailerProducts
                                    .Include( i => i.ClientRetailer)
                                    .ThenInclude( ti => ti.Retailer)
                                    .ToListAsync();
            return View(products);
        }


        public async Task<IActionResult> FindByUPC(string upc)
        {
            // Transformation: Occasionally UPC may have 
            // extra leading zeros
            if (upc.Length > 12)
                upc = upc.Substring(upc.Length-12);

            ClientRetailerProduct crProduct = await _db.ClientRetailerProducts.FirstOrDefaultAsync( q => q.UPC == upc);
            if(crProduct == null)
                return NotFound();

            Product_UPCSearchResults retval = new Product_UPCSearchResults
            {
                Product_Name = crProduct.Name,
                ClientRetailerProductId = crProduct.Id,
                Cost = crProduct.Cost
            };
            return PartialView("_UPCSearchResult", retval);
        }

    }

    public class Product_UPCSearchResults
    {
        public string Product_Name { get; set; }
        public decimal? Cost { get; set; }
        public int ClientRetailerProductId { get; set; }
    }
}