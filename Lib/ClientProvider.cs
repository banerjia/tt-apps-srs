using Microsoft.AspNetCore.Http;
using System.Linq;
using tt_apps_srs.Models;

namespace tt_apps_srs.Lib
{
    public class ClientProvider : IClientProvider
    {
        private int? _clientId; 

        public ClientProvider(IHttpContextAccessor accessor, tt_apps_srs_db_context db)
        {
            var host = accessor.HttpContext.Request.Path.ToString().Split('/');

            _clientId = db.Clients.FirstOrDefault(q => q.UrlCode == host[0])?.Id;
        }

        public int? GetClientId()
        {
            return _clientId;
        }
    }
}
