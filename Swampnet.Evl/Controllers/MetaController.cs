using Microsoft.AspNetCore.Mvc;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("api/meta")]
    public class MetaController : Controller
    {
        // GET api/meta
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MetaData.Default);
        }
    }
}
