using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Swampnet.Evl.Web.Controllers
{
    public class HomeController : Controller
    {
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
    }

	public class About
	{
		public string Tmp { get; set; }
	}
}
