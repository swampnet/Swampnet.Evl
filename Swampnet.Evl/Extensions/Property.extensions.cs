using Microsoft.AspNetCore.Http;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    static class PropertyExtensions
    {
        public static void Add(this ICollection<Property> properties, HttpContext context)
        {
            properties.Add(new Property("Internal", "Remote Ip Address", context.Connection.RemoteIpAddress.ToString()));
        }
    }
}
