using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using tt_apps_srs.Lib;

namespace tt_apps_srs.Models.ES
{
    public class ESIndex_Order : IESIndex
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
}
