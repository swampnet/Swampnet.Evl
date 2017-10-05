using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web
{
    /// <summary>
    /// Client side configuration
    /// </summary>
    public class Cfg
    {
        public Cfg(IConfiguration configuration)
        {
            ApiRoot = "http://localhost:5000/";
        }


        public string ApiRoot { get; set; }
    }
}
