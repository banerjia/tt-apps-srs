using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly tt_apps_srs_db_context _db;

        public IndexModel(tt_apps_srs_db_context db)
        {
            _db = db;
            var t = db.Tenants.First();
            t.UrlCode = "test_es_Z";

            _db.Attach(t).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _db.SaveChangesAsync();
        }

        public void OnGet()
        {

        }
    }
}
