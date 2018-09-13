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

    }
}