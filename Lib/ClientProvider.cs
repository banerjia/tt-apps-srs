using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Text;
using tt_apps_srs.Models;

namespace tt_apps_srs.Lib
{
    public class ClientProvider : IClientProvider
    {
        private int _clientId;
        private string _clientName;
        private string _clientUrlCode;

        public ClientProvider(IHttpContextAccessor accessor, tt_apps_srs_db_context db, IDistributedCache cache)
        {
            var host = accessor.HttpContext.Request.Path.ToString().Split('/');
            string _clientUrlCode = host[1];

            if(_clientUrlCode.Equals("Manage", StringComparison.OrdinalIgnoreCase))
            {
                _clientId = 0;
                _clientName = "SRS";
                return;
            }

            var cacheEntry = cache.Get(_clientUrlCode + "_name");
            if(cacheEntry == null || !cacheEntry.Any())
            {
                var client = db.Clients.FirstOrDefault(q => q.UrlCode == _clientUrlCode);
                _clientId = client.Id;
                _clientName = client.Name;

                cache.SetAsync(_clientUrlCode + "_name", Encoding.ASCII.GetBytes(_clientName));
                cache.SetAsync(_clientUrlCode + "_id", BitConverter.GetBytes(_clientId));

            }
            else
            {
                _clientName = Encoding.ASCII.GetString(cacheEntry);
                cacheEntry = cache.Get(_clientUrlCode + "_id");
                _clientId = BitConverter.ToInt32(cacheEntry,0);
            }

            
        }

        public int ClientId
        {
            get
            {
                return _clientId;
            }

        }
            
        public string Name
        {
            get
            {
                return _clientName;
            }
        }

        public string UrlCode
        {
            get
            {
                return _clientUrlCode;
            }
        }
    }
}
