using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using tt_apps_srs.Lib;
using tt_apps_srs.Models;

namespace tt_apps_srs.Models.ES
{
    public class ESIndex_Order : IESIndex
    {
        private readonly ElasticClient _es;
        private const string ES_INDEX_NM = "tt-apps-srs-orders";
        private const string ES_INDEX_TYP_NM = "order";

        public ESIndex_Order(ElasticClient esSvcClient)
        {

            _es = esSvcClient;
            if (!_es.IndexExists(Indices.Index(ES_INDEX_NM)).Exists)
            {
                CreateMapping(_es);
            }

        }

        private void CreateMapping(ElasticClient es)
        {

            es.CreateIndexAsync(ES_INDEX_NM, c => c
                    .Mappings(ms => ms
                       .Map<ES_Order_Document>(ES_INDEX_TYP_NM, m => m
                         .Properties(prop => prop
                            .Text(s => s
                               .Name(e => e.Id)
                               .Index(false)
                            )
                            .Number( s => s
                                .Name( e => e.Total)
                                .Type(NumberType.Double)
                            )
                            .Object<ES_Order_Document_Client>(s => s
                               .Name(n => n.Client)
                               .Properties(props => props
                                  .Text(s1 => s1
                                     .Name(e => e.UrlCode)
                                  )
                                  .Number(s1 => s1
                                     .Name(e => e.ClientId)
                                     .Type(NumberType.Integer)
                                     .Index(false)
                                  )
                               )
                            )
                            .Object<ES_Order_Document_Store>(s => s
                               .Name(n => n.Store)
                               .Properties(props => props
                                  .Text(s1 => s1
                                     .Name(e => e.StoreName)
                                     .Analyzer("standard")
                                  )
                                  .Text(s1 => s1
                                     .Name(e => e.StoreId)
                                     
                                  )
                                  .Text(s1 => s1
                                    .Name(e => e.Agg_StoreName)
                                    .Fields(fs => fs
                                           .Keyword(ss => ss.Name("raw")
                                                   )
                                           )
                                  )
                               )
                            )
                         )
                       )
                    )
                );
        }

        public async void CreateAsAsync(object document)
        {
            ClientStoreOrder order = (ClientStoreOrder)document;

            ES_Order_Document order_to_add = new ES_Order_Document
            {
                Id = order.Id,
                Total = order.Total,
                Client = new ES_Order_Document_Client
                {
                    ClientId = order.ClientStore.ClientId,
                    UrlCode = order.ClientStore.Client.UrlCode
                },
                Store = new ES_Order_Document_Store
                {
                    StoreId = order.ClientStore.StoreId,
                    StoreName = order.ClientStore.Store.Name,
                    Agg_StoreName = order.ClientStore.Store.Name
                },
                Creation = new ES_Order_Document_Creation
                {
                    UserId = order.CreatedBy,
                    CreatedAt = order.CreatedAt

                }
            };

            await _es.IndexAsync(order_to_add,
                i => i
                        .Index(ES_INDEX_NM)
                        .Type(ES_INDEX_TYP_NM)
            );
        }

        public async void DeleteIndex()
        {
            await _es.DeleteIndexAsync(ES_INDEX_NM);
        }

        public async void RemoveAsAsync(object id)
        {
            Guid order_id = (Guid)id;
            await _es.DeleteAsync(new DeleteRequest<ES_Order_Document>(
                                            ES_INDEX_NM,
                                            ES_INDEX_TYP_NM,
                                            order_id)
            {
                Refresh = Elasticsearch.Net.Refresh.True
            }

            );
        }

        public ISearchResponse<T> Search<T>(ISearchRequest searchCriteria) where T : class
        {
            ISearchRequest searchConfig = new SearchRequest<ES_Order_Document>(
                                                                              Indices.Index(ES_INDEX_NM),
                                                                              ES_INDEX_TYP_NM)
            {
                Query = searchCriteria.Query,
                Aggregations = searchCriteria.Aggregations,
                From = searchCriteria.From,
                Size = searchCriteria.Size,
                Sort = searchCriteria.Sort
            };


            var retval = _es.Search<T>(searchConfig);

            return retval;
        }

        public async Task<IReadOnlyCollection<object>> SearchAsync(ISearchRequest searchCriteria, uint skip = 0, ushort results_per_fetch = 10)
        {
            IReadOnlyCollection<ES_Order_Document> retval;

            ISearchRequest searchConfig = new SearchRequest<ES_Order_Document>(
                                                                            Indices.Index(ES_INDEX_NM),
                                                                            ES_INDEX_TYP_NM)
            {
                Query = searchCriteria.Query,
                Aggregations = searchCriteria.Aggregations,
                From = (int)skip,
                Size = results_per_fetch
            };

            var searchResponse = await _es.SearchAsync<ES_Order_Document>(searchConfig);

            retval = searchResponse.Documents;

            return retval;
        }

        public async Task<ISearchResponse<T>> SearchAsync<T>(ISearchRequest searchCriteria) where T : class
        {
            ISearchRequest searchConfig = new SearchRequest<ES_Order_Document>(
                                                                              Indices.Index(ES_INDEX_NM),
                                                                              ES_INDEX_TYP_NM)
            {
                Query = searchCriteria.Query,
                Aggregations = searchCriteria.Aggregations,
                From = searchCriteria.From,
                Size = searchCriteria.Size,
                Sort = searchCriteria.Sort
            };


            var retval = await _es.SearchAsync<T>(searchConfig);

            return retval;
        }
    }

    public class ES_Order_Document
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public ES_Order_Document_Creation Creation { get; set; }

        public ES_Order_Document_Store Store { get; set; }
        public ES_Order_Document_Client Client { get; set; }

    }

    public class ES_Order_Document_Creation
    {
        public Guid UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ES_Order_Document_Store
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public string Agg_StoreName { get; set; }

    }

    public class ES_Order_Document_Client
    {
        public int ClientId { get; set; }
        public string UrlCode { get; set; }

    }
}
