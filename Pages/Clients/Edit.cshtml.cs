using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages.Clients
{
    public class EditModel : PageModel
    {
        public readonly tt_apps_srs_db_context _db;

        public EditModel(tt_apps_srs_db_context db)
        {
            _db = db;
        }

        [BindProperty]
        public Client Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client = await _db.Clients.FindAsync(id);

            if (Client == null)
                return RedirectToPage("./Index");

            return Page();
        }

        //[Bind(new string[] { "Name", "Id", "UrlCode" })] Client client
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Attach(Client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch(Exception)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}