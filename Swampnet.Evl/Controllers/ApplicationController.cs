using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
	[Route("application")]
	public class ApplicationController : Controller
    {
		private readonly IManagementDataAccess _managementData;

		public ApplicationController(IManagementDataAccess managementData)
		{
			_managementData = managementData;
		}

		//[HttpGet]
		//public async Task<IActionResult> Get()
		//{
		//	try
		//	{
		//		// @TODO: Auth
		//		var org = await _managementData.LoadOrganisationAsync();

		//		return Ok(org.Applications);
		//	}
		//	catch (Exception ex)
		//	{
		//		Log.Error(ex, ex.Message);
		//		return this.InternalServerError(ex);
		//	}
		//}


		//[HttpGet("{code}")]
		//public async Task<IActionResult> Get(string code)
		//{
		//	try
		//	{
		//		// @TODO: Auth
		//		var org = await _managementData.LoadOrganisationAsync();
		//		var app = await _managementData.LoadApplicationAsync(org.Id, code);

		//		return Ok(app);
		//	}
		//	catch (Exception ex)
		//	{
		//		Log.Error(ex, ex.Message);
		//		return this.InternalServerError(ex);
		//	}
		//}
	}
}
