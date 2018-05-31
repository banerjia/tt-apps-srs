using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages.Client
{
    public class IndexModel : PageModel
    {
        private readonly tt_apps_srs_db_context _db;
        private readonly IClientProvider _clientProvider;

        public IndexModel(tt_apps_srs_db_context db, IClientProvider clientProvider)
        {
            _db = db;
            _clientProvider = clientProvider;
        }

        public IList<tt_apps_srs.Models.Client> Clients { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            int? client_id = _clientProvider.GetClientId();
        
            Clients = await _db.Clients.Where(q => q.Active ).ToListAsync();

            return Page();
        }
    }
}
