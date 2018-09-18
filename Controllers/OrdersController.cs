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

    }
}