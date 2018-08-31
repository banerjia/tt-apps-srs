using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using tt_apps_srs.Lib;
using System.Collections.ObjectModel;

namespace tt_apps_srs.Models
{
    public class ESIndex_Store : IESIndex
    {
        private readonly ElasticClient _es;

        public ESIndex_Store()
        {
            var connectionString = "http://localhost:9200";
            var connectionConfiguration = new ConnectionSettings(new Uri(connectionString))                                      
                                        .DefaultMappingFor<ESIndex_Store_Document>(i => i
                                                                        .IndexName("tt-apps-srs-stores")
                                                                        .TypeName("store")                                                                        
                                                                        );

            _es = new ElasticClient(connectionConfiguration);


        }

        public async void CreateAsAsync(object document)
        {
            Store store = (Store)document;
            
            if(store.Longitude == 0 || store.Latitude == 0)
            {
                string address = String.Format("{0},{1},{2}-{3}, US", store.Addr_Ln_1, store.City, store.State, store.Zip);
                GoogleGeocoding_Location location = GeneralPurpose.GetLatLong(address);
                store.Latitude = location.lat;
                store.Longitude = location.lng;
            }

            ESIndex_Store_Document store_to_add = new ESIndex_Store_Document
            {
                Id = store.Id,
                Name = store.Name,
                Location = new GeoLocation(store.Latitude ?? 0, store.Longitude ?? 0),
                City = store.City,
                State = store.State,
                Retailer = new ESIndex_Store_Document_Retailer{
                    Id = store.RetailerId,
                    Name = store.Retailer.Name,
                    Agg_Name =String.Format("{0}={1}", store.Retailer.Name, store.RetailerId)
                }
                
            };
            store_to_add.Clients = 
                store.ClientStores.Select( s => new ESIndex_Store_Document_Client{
                UrlCode = s.Client.UrlCode,
                Name = s.Client.Name,
                CreatedAt = s.CreatedAt
            }).ToArray<ESIndex_Store_Document_Client>();
            await _es.IndexDocumentAsync(store_to_add);
        }

        public async void RemoveAsAsync(object id)
        {
            Guid store_id = (Guid)id;
            await _es.DeleteAsync<ESIndex_Store_Document>(store_id);
        }

        public async Task<IReadOnlyCollection<object>> SearchAsync(object searchCriteria, uint skip = 0, ushort results_per_fetch = 10)
        {
            IReadOnlyCollection<ESIndex_Store_Document> retval;

            var searchResponse = await _es.SearchAsync<ESIndex_Store_Document>(s => s
                                            .Query( q => q.MatchAll())
                                            .Skip( (int)skip )
                                            .Take(results_per_fetch)
            );

            retval = searchResponse.Documents;

            return retval;
        }
        public async Task<ISearchResponse<T>> SearchAsync<T>(ISearchRequest query) where T : class
        {
            var retval = await _es.SearchAsync<T>(query);

            return retval;
        }
    }


    public class ESIndex_Store_Document
    {
        [Text(Analyzer = "")]
        public Guid Id {get;set;}
        public string Name { get; set; }
        public string City { get; set; }
        public string State {get;set;}
        public GeoLocation Location { get; set; }

        public ESIndex_Store_Document_Client[] Clients { get; set; }
        public ESIndex_Store_Document_Retailer Retailer { get; set; }

    }

    public class ESIndex_Store_Document_Client
    {
        public string UrlCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ESIndex_Store_Document_Retailer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Agg_Name {get;set;}
    }
}
