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
    public class ESIndex_Product : IESIndex
    {
        public void CreateAsAsync(object document)
        {
            throw new NotImplementedException();
        }

        public void DeleteIndex()
        {
            throw new NotImplementedException();
        }

        public void RemoveAsAsync(object id)
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<T> Search<T>(ISearchRequest query) where T : class
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
    }

    public class ESIndex_Product_Document
    {
        
    }
}