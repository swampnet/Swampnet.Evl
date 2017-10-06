using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            Log.Information("GET: Index");
            return View(new About());
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }


        public IActionResult Cfg()
        {
            return Ok(new Cfg(_configuration));
        }
    }



	public class About
	{
		public string Tmp { get; set; }
	}
}
