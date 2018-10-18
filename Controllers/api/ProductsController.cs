using tt_apps_srs.Models;
using tt_apps_srs.Lib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tt_apps_srs.api
{
    [Route("{client_url_code}/api/[controller]/[action]")]
    [ApiController]
    public class ProductsController:ControllerBase
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly IClientProvider _client;
        public ProductsController(tt_apps_srs_db_context db,
                                IClientProvider client
        )
        {
            _db = db;
            _client = client;
        }
    }
}