using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tt_apps_srs.Lib
{
    public interface IClientProvider
    {
        int? ClientId { get; }
        string UrlCode { get; }
        string Name { get; }
    }
}
