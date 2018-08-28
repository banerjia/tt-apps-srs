using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using tt_apps_srs.Lib;

namespace tt_apps_srs.Models
{
    public class ES_Index_Store : I_ES_Index
    {
        private readonly ElasticClient _client;

        public ES_Index_Store()
        {
            var connectionString = "http://localhost:9200";
            var connectionConfiguration = new ConnectionSettings(new Uri(connectionString))
                                        .DefaultMappingFor<ES_Index_Store_Document>(i => i
                                                                        .IndexName("tt-apps-srs")
                                                                        .TypeName("store"));

            _client = new ElasticClient(connectionConfiguration);


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

            ES_Index_Store_Document store_to_add = new ES_Index_Store_Document
            {
                id = store.Id,
                name = store.Name,
                location = new GeoLocation(store.Latitude ?? 0, store.Longitude ?? 0),
                city = store.City,
                state = store.State
            };
            await _client.IndexDocumentAsync(store_to_add);
        }

        public async void RemoveAsAsync(object id)
        {
            Guid store_id = (Guid)id;
            await _client.DeleteAsync<ES_Index_Store_Document>(store_id);
        }
    }


    public class ES_Index_Store_Document
    {
        public Guid id { get; set; }

        public int[] clients { get; set; }

        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public GeoLocation location { get; set; }
    }
}
