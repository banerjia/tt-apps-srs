using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace tt_apps_srs.Controllers
{
    public class StoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}