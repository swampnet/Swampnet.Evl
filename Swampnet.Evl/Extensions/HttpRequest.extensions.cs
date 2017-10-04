using Microsoft.AspNetCore.Http;
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
            return Guid.Parse(rq.Headers[Constants.API_KEY_HEADER].SingleOrDefault());
        }


        public static IEnumerable<Property> CommonProperties(this HttpRequest rq)
        {
            var properties = new List<Property>();

            properties.Add(new Property("Internal", "Remote Ip Address", rq.RemoteIpAddress()));

            return properties;
        }
    }
}
