using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages.Tenant
{
    public class IndexModel : PageModel
    {
        private readonly tt_apps_srs_db_context _db;

        public IndexModel(tt_apps_srs_db_context db)
        {
            _db = db;
        }

        public IList<tt_apps_srs.Models.Tenant> Tenants { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Tenants = await _db.Tenants.Where(q => q.Active).ToListAsync();

            return Page();
        }
    }
}
