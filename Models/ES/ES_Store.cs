using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tt_apps_srs.Models
{
    public class ESStore : I_ESStore
    {
        private readonly ElasticClient _client;

        public ESStore()
        {
            var connectionString = "http://localhost:9200";
            var connectionConfiguration = new ConnectionSettings(new Uri(connectionString))
                                        .DefaultMappingFor<ES_Store>(i => i
                                                                        .IndexName("tt-apps-srs")
                                                                        .TypeName("store"));

            _client = new ElasticClient(connectionConfiguration);


        }

        public async void CreateAsAsync(Store store)
        {
            await _client.IndexDocumentAsync(store);
        }

        public void RemoveAsAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
