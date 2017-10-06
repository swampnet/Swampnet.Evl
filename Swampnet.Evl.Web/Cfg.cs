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
            //this._baseUrl = "http://localhost:5001/";
            //this._baseUrl = "http://localhost:5000/";
            //this._baseUrl = "http://swampnet-evl-staging.azurewebsites.net/api/";

            ApiRoot = configuration["client:apiRoot"];
        }

        public string ApiRoot { get; set; }
    }
}
