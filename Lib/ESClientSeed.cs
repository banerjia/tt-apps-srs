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
            ElasticClient esSvcClient = app.ApplicationServices.GetService<ElasticClient>();
            
            ESIndex_Client clientIndex = new ESIndex_Client(esSvcClient);
            clientIndex.CreateAsAsync( new Client {Id = 1, Name = "ICU", UrlCode="icu"});
        }
    }
}
