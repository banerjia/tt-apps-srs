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
                                                                        .IndexName("tt-apps-srs")
                                                                        .TypeName("store"));

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

            ESIndex_Store_Document store_to_add = (ESIndex_Store_Document) store;
            store_to_add.location = new GeoLocation(store.Latitude ?? 0, store.Longitude ?? 0);
            /*
            new ESIndex_Store_Document
            {
                Id = store.Id,
                name = store.Name,
                location = new GeoLocation(store.Latitude ?? 0, store.Longitude ?? 0),
                city = store.City,
                state = store.State
            };*/
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
        /*
        public async Task<ISearchResponse<object>> SearchAsync(ISearchRequest query)
        {

            var retval = await _es.SearchAsync<object>(query);

            return retval;
        }
        */
        public async Task<ISearchResponse<T>> SearchAsync<T>(ISearchRequest query) where T : class
        {

            var retval = await _es.SearchAsync<T>(query);

            return retval;
        }
    }


    public class ESIndex_Store_Document:Store
    {
        public GeoLocation location { get; set; }
    }
}
