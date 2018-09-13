using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
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

        private readonly IESIndex _esClient;

        public ClientProvider(IHttpContextAccessor accessor, 
                                IDistributedCache cache, 
                                ElasticClient esSvcClient)
        {

            _esClient = new ESIndex_Client(esSvcClient);
            
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

                var esResponse = _esClient.Search<ESIndex_Client_Document>(
                    new SearchRequest<ESIndex_Client_Document>{
                        Query = new BoolQuery{
                            Filter = new List<QueryContainer>{
                                new TermQuery{
                                    Field = "urlCode",
                                    Value = _clientUrlCode
                                }
                            }
                        }, 
                        Size = 1
                    }
                );

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
