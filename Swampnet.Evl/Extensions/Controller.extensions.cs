using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    /// <summary>
    /// WebApi controller helpers
    /// </summary>
    static class ControllerExtensions
    {
        public static ObjectResult InternalServerError(this Controller controller, Exception ex)
        {
            return controller.StatusCode((int)HttpStatusCode.InternalServerError, ex);
        }
    }
}
