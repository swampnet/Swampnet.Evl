using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Mocks;

namespace UnitTests
{
	[TestClass]
    public class PermissionTests
    {
		[TestMethod]
		public void Permissions_Basic()
		{
			var profile = Mock.Profile();

			Assert.IsTrue(profile.HasRole("test-role"));
			Assert.IsTrue(profile.HasPermission("test.permission"));
		}
    }
}
