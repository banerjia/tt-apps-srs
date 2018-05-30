using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages.Tenant
{
    public class CreateModel : PageModel
    {
        private readonly tt_apps_srs_db_context _db;

        public CreateModel(tt_apps_srs_db_context db)
        {
            _db = db;
        }

        [BindProperty]
        public TenantCreateModel Tenant {get;set;}

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(TenantCreateModel tenant)
        {
            if (!ModelState.IsValid)
                return Page();

            tt_apps_srs.Models.Tenant addTenant = new tt_apps_srs.Models.Tenant{
                Name = tenant.Name,
                UrlCode = tenant.UrlCode,
                Properties =  tenant.Properties
            };

            _db.Tenants.Add(addTenant);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    
    }

    public class TenantCreateModel
    {
        public TenantCreateModel()
        {
        }

        [BindProperty]
        [DisplayName("Tenant Name")]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [DisplayName("Tenant Url Part")]
        [Required]
        public string UrlCode { get; set; }

        public string Properties { get; set; }
    }
}
