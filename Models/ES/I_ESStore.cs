using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace tt_apps_srs.Models
{
    interface I_ESStore
    {
        void CreateAsAsync(Store store);
        void RemoveAsAsync(Guid id);
    }

    public class ES_Store
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        
        public GeoLocation location { get; set; }
    }
}
