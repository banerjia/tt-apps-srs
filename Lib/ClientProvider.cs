using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
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

        private readonly ElasticClient _esClient;

        public ClientProvider(IHttpContextAccessor accessor, IDistributedCache cache, IConfiguration config)
        {
            
            try{
            var host = accessor.HttpContext.Request.Path.ToString().Split('/');
            _clientUrlCode = host[1];

            }
            catch{
                _clientUrlCode = "icu";
            }

            if(_clientUrlCode.Equals("Manage", StringComparison.OrdinalIgnoreCase))
            {
                _clientId = 0;
                _clientName = "SRS";
                return;
            }

            var cacheEntry = cache.Get(_clientUrlCode + "_name");
            if(cacheEntry == null || !cacheEntry.Any())
            {
                string connectionString = config.GetConnectionString("DefaultESConnection");
                var connectionConfiguration = new ConnectionSettings(new Uri(connectionString))
                                                                            .DefaultMappingFor<Client>(i => i
                                                                            .IndexName("tt-apps-srs-clients")
                                                                            );

                _esClient = new ElasticClient(connectionConfiguration);

                var esResponse = _esClient.Search<ESIndex_Client_Document>
                ( s => s
                        .Query( q => new TermQuery{
                                                   Field = "UrlCode",
                                                   Value = _clientUrlCode
                         })
                         .Take(1));

                var client = esResponse.Documents.First();
                _clientName = client.Name;
                _clientId = client.Id;

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
