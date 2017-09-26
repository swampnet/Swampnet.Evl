using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Services;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class StringExtensionTests
    {
		[TestMethod]
		public void String_EqualsNoCase_BothNull()
		{
			string lhs = null;
			string rhs = null;

			Assert.IsTrue(lhs.EqualsNoCase(rhs));
		}

		[TestMethod]
		public void String_EqualsNoCase_OneSideNull()
		{
			string lhs = "some-value";
			string rhs = null;

			Assert.IsFalse(lhs.EqualsNoCase(rhs));
		}

		[TestMethod]
		public void String_EqualsNoCase_01()
		{
			string lhs = "some-value";
			string rhs = "SOME-VALUE";

			Assert.IsTrue(lhs.EqualsNoCase(rhs));
		}

        [TestMethod]
        public void String_AsX()
        {
            string source = "True";

            Assert.AreEqual(true, source.As<bool>());

            source = "1.234";
            Assert.AreEqual(1.234, source.As<double>());
        }


        [TestMethod]
        public void AsString()
        {
            double i = 1.234;
            Assert.AreEqual("1.234", i.AsString());
        }
    }
}
