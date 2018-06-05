using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tt_apps_srs.Models;

namespace tt_apps_srs.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly tt_apps_srs_db_context _db;

        public EditModel(tt_apps_srs_db_context db)
        {
            _db = db;
        }

        [BindProperty]
        public ClientEditModel Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client client = await _db.Clients.FindAsync(id);

            if (client == null)
                return RedirectToPage("./Index");

            Client = new ClientEditModel
            {
                Id = client.Id,
                Name = client.Name,
                UrlCode = client.UrlCode,
                Active = client.Active,
                Properties = client.Properties?.ToString()
            };
 
            return Page();
        }

        //[Bind(new string[] { "Name", "Id", "UrlCode" })] Client client
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Client clientToUpdate = new Client
            {
                Id = Client.Id,
                Name = Client.Name,
                UrlCode = Client.UrlCode,
                Active = Client.Active,
                Properties = Client.Properties
            };

            _db.Attach(clientToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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

    public class ClientEditModel
    {
        public ClientEditModel()
        {
        }

        public int Id { get; set; }

        [DisplayName("Client Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Client Url Part")]
        [Required]
        public string UrlCode { get; set; }

        public string Properties { get; set; }

        public bool Active { get; set; }
    }
}