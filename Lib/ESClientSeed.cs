using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tt_apps_srs.Models;
using Nest;
using Microsoft.Extensions.DependencyInjection;

namespace tt_apps_srs.Lib
{
    public static class ESClientSeed
    {
        public static void SeedIndex(IApplicationBuilder app)
        {
            ElasticClient esClient = app.ApplicationServices.GetService<ElasticClient>();
            
            {
                esClient.DeleteIndex("tt-apps-srs-clients");
                esClient.CreateIndex("tt-apps-srs-clients", c => c
                                                            .Mappings( ms => ms
                                                                .Map<ESIndex_Client_Document>( m => m.AutoMap<Client>()))
 
                );
                {
                    esClient.IndexAsync<ESIndex_Client_Document>(    
                        new ESIndex_Client_Document
                        {
                            Id = 1,
                            Name = "ICU",
                            UrlCode = "icu"
                        },
                        i => i
                            .Index("tt-apps-srs-clients")
                            .Refresh(Elasticsearch.Net.Refresh.True)
                        );
                }

            }
        }
    }
}
