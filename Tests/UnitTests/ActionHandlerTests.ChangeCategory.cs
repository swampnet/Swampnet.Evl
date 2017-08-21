using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ChangeCategoryActionHandlerTests
    {
        [TestMethod]
        public void Action_ChangeCategory()
        {
            var evt = Mock.Event();
            var expected = "new-category";

            var changeCategoryAction = new ChangeCategoryActionHandler();

            changeCategoryAction.Apply(
                evt, 
                new[] 
                {
                    new Property("category", expected)
                });

            var actual = evt.Category;

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Action_ChangeCategory_MissingArgs()
        {
            var evt = Mock.Event();
            var expected = evt.Category;

            var changeCategoryAction = new ChangeCategoryActionHandler();

            changeCategoryAction.Apply(
                evt,
                new[]
                {
                    new Property("some-unrelated-property", "some unrelated value")
                });

            var actual = evt.Category;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Action_ChangeCategory_NullArgs()
        {
            var evt = Mock.Event();
            var expected = evt.Category;

            var changeCategoryAction = new ChangeCategoryActionHandler();

            changeCategoryAction.Apply(
                evt,
                null);

            var actual = evt.Category;

            Assert.AreEqual(expected, actual);
        }
    }
}
