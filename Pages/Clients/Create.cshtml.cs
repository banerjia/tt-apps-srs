using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages.Client
{
    public class CreateModel : PageModel
    {
        private readonly tt_apps_srs_db_context _db;

        public CreateModel(tt_apps_srs_db_context db)
        {
            _db = db;
        }

        [BindProperty]
        public ClientCreateModel Client {get;set;}

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(ClientCreateModel tenant)
        {
            if (!ModelState.IsValid)
                return Page();

            tt_apps_srs.Models.Client addClient = new tt_apps_srs.Models.Client{
                Name = tenant.Name,
                UrlCode = tenant.UrlCode,
                Properties =  tenant.Properties
            };

            _db.Clients.Add(addClient);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    
    }

    public class ClientCreateModel
    {
        public ClientCreateModel()
        {
        }

        [BindProperty]
        [DisplayName("Client Name")]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [DisplayName("Client Url Part")]
        [Required]
        public string UrlCode { get; set; }

        public string Properties { get; set; }
    }
}
