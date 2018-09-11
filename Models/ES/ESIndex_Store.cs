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
using Microsoft.Extensions.Configuration;
using tt_apps_srs.Models;

namespace tt_apps_srs.Lib
{
    public class ESIndex_Store : IESIndex
    {
        private readonly ElasticClient _es;
        private const string ES_INDEX_NM = "tt-apps-srs-stores";
        private const string ES_INDEX_TYP_NM = "esindex_store_document";

        public ESIndex_Store(ElasticClient esSvcClient)
        {
            
            _es = esSvcClient;
            if(!_es.IndexExists(Indices.Index(ES_INDEX_NM)).Exists)
            {
                CreateMapping(_es);
            }
            else
            {
            }

        }

        private void CreateMapping(ElasticClient es)
        {
            es.CreateIndexAsync(ES_INDEX_NM, c => c
                    .Mappings(ms => ms
                       .Map<ESIndex_Store_Document>(m => m
                          .Properties(prop => prop
                             .Text(s => s
                                .Name(e => e.Id)
                                .Index(false)
                             )
                             .Text(s => s
                                .Name(e => e.Name)
                                .Analyzer("standard")
                             )
                             .Text(s => s
                                .Name(e => e.City)
                                .Analyzer("standard")
                             )
                             .Text(s => s
                                .Name(e => e.State)
                             )
                             .GeoPoint(s => s
                                .Name(e => e.Location)
                             )
                             .Object<ESIndex_Store_Document_Client>(s => s
                                .Name(n => n.Clients)
                                .Properties(props => props
                                   .Text(s1 => s1
                                      .Name(e => e.Name)
                                      .Index(false)
                                   )
                                   .Text(s1 => s1
                                      .Name(e => e.UrlCode)
                                   )
                                   .Number(s1 => s1
                                      .Name(e => e.LocationNumber)
                                      .Type(NumberType.Integer)
                                   )
                                )
                             )
                             .Object<ESIndex_Store_Document_Retailer>(s => s
                                .Name(n => n.Retailer)
                                .Properties(props => props
                                   .Text(s1 => s1
                                      .Name(e => e.Name)
                                      .Analyzer("standard")
                                   )
                                   .Text(s1 => s1
                                      .Name(e => e.Id)
                                      .Index(false)
                                   )
                                   .Text(s1 => s1
                                      .Name(e => e.Agg_Name)
                                   )
                                )
                             )
                          )
                       )
                    )
                ); ;
        }

        public async void CreateAsAsync(object document)
        {

            if (!_es.IndexExists(Indices.Index(ES_INDEX_NM)).Exists)
            {
                CreateMapping(_es);
            }

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

            await _es.IndexAsync(store_to_add,
                i => i
                        .Index(ES_INDEX_NM)
            );
        }

        public async void RemoveAsAsync(object id)
        {
            Guid store_id = (Guid)id; 
            await _es.DeleteAsync(new DeleteRequest<ESIndex_Store_Document>(
                                            ES_INDEX_NM, 
                                            ES_INDEX_TYP_NM,
                                            store_id)
                                    {
                                        Refresh = Elasticsearch.Net.Refresh.True
                                    }
                                            
            );
        }

        public async Task<IReadOnlyCollection<object>> SearchAsync(ISearchRequest searchCriteria, uint skip = 0, ushort results_per_fetch = 10)
        {
            IReadOnlyCollection<ESIndex_Store_Document> retval;

            ISearchRequest searchConfig = new SearchRequest<ESIndex_Store_Document>(
                                                                            Indices.Index(ES_INDEX_NM))
            {
                Query = searchCriteria.Query,
                Aggregations = searchCriteria.Aggregations,
                From = (int)skip,
                Size = results_per_fetch
            };

            var searchResponse = await _es.SearchAsync<ESIndex_Store_Document>(searchConfig);

            retval = searchResponse.Documents;

            return retval;
        }
        public async Task<ISearchResponse<T>> SearchAsync<T>(ISearchRequest searchCriteria) where T : class
        {
            ISearchRequest searchConfig = new SearchRequest<ESIndex_Store_Document>(
                                                                              Indices.Index(ES_INDEX_NM)) {
                Query = searchCriteria.Query,
                Aggregations = searchCriteria.Aggregations,
                From = searchCriteria.From,
                Size = searchCriteria.Size,
                Sort = searchCriteria.Sort
            };


            var retval = await _es.SearchAsync<T>(searchConfig);            

            return retval;
        }

        public void DeleteIndex()
        {
            _es.DeleteIndexAsync(ES_INDEX_NM);
        }
    }


    public class ESIndex_Store_Document
    {
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
        public int? LocationNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ESIndex_Store_Document_Retailer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Agg_Name {get;set;}
    }
}
