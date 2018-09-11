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


    public class ESIndex_Client:IESIndex
    {
        private readonly ElasticClient _es;
        private const string ES_INDEX_NM = "tt-apps-srs-clients";
        private const string ES_INDEX_TYP_NM = "client";
        public ESIndex_Client(ElasticClient esSvcClient)
        {
            _es = esSvcClient;
            if(!_es.IndexExists(Indices.Index(ES_INDEX_NM)).Exists)
            {
                _es.CreateIndexAsync(ES_INDEX_NM, c => c
                    .Mappings(ms => ms                        
                        .Map<ESIndex_Client_Document>( m => m                            
                            .Properties( ps => ps
                                .Text(s => s
                                    .Name( e => e.Name))
                                .Text( s => s
                                    .Name( e => e.UrlCode)
                                )
                                .Number( s => s
                                    .Name( e => e.Id)
                                    .Type(NumberType.Integer)                                    
                                )

                            )
                        )
                    )                    
                
                );
            }
        }

        public void CreateAsAsync(object document)
        {
            Client client = (Client)document;

            ESIndex_Client_Document client_to_add = new ESIndex_Client_Document{
                Id = client.Id,
                Name = client.Name,
                UrlCode = client.UrlCode
            };

            _es.IndexAsync( client_to_add, 
                i => i
                    .Index(ES_INDEX_NM)
            );
        }

        public void RemoveAsAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<object>> SearchAsync(ISearchRequest searchCriteria, uint skip = 0, ushort results_per_fetch = 10)
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<T>> SearchAsync<T>(ISearchRequest query) where T : class
        {
            throw new NotImplementedException();
        }

        public void DeleteIndex()
        {
            _es.DeleteIndexAsync(ES_INDEX_NM);
        }
    }
    public class ESIndex_Client_Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlCode { get; set; }

    }
}
