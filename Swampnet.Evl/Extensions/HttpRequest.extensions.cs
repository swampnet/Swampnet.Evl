using Microsoft.AspNetCore.Http;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    /// <summary>
    /// HttpRequest helpers
    /// </summary>
    static class HttpRequestExtensions
    {
        public static string RemoteIpAddress(this HttpRequest rq)
        {
            return rq.HttpContext.Connection.RemoteIpAddress.ToString();
        }


        public static Guid ApiKey(this HttpRequest rq)
        {
            Guid apiKey = Guid.Empty;
            if(!Guid.TryParse(rq.Headers[Constants.API_KEY_HEADER].SingleOrDefault(), out apiKey))
            {
                Log.Information("Can't resolve api-key");
            }
            return apiKey;
        }


        public static IEnumerable<Property> CommonProperties(this HttpRequest rq)
        {
            var properties = new List<Property>();

            if(rq != null)
            {
                properties.Add(new Property("Internal", "Remote Ip Address", rq.RemoteIpAddress()));
            }

            return properties;
        }
    }
}
