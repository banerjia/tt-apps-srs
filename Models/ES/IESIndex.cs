﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace tt_apps_srs.Models
{
    interface IESIndex
    {
        void CreateAsAsync(object document);
        void RemoveAsAsync(object id);

        Task<IReadOnlyCollection<object>> SearchAsync(object searchCriteria, uint skip = 0, ushort results_per_fetch = 10);
        Task<ISearchResponse<T>> SearchAsync<T>(ISearchRequest query) where T:class;

    }
}