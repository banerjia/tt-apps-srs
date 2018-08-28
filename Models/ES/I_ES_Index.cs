using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace tt_apps_srs.Models
{
    interface I_ES_Index
    {
        void CreateAsAsync(object document);
        void RemoveAsAsync(object id);
    }
}
