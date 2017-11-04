using UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace UnitTests
{
    [TestClass]
    public class ChangeCategoryActionHandlerTests
    {
        [TestMethod]
        public void Action_ChangeCategory()
        {
            var evt = Mock.Event();
            var expected = EventCategory.Warning;

            var changeCategoryAction = new ChangeCategoryActionHandler();

            changeCategoryAction.ApplyAsync(
                evt, 
				new ActionDefinition()
				{
					Properties = new[]
					{
						new Property("category", expected)
					}
				},
				new Rule() { Name = "Mocked Rule" }).Wait();

            var actual = evt.Category;

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Action_ChangeCategory_MissingArgs()
        {
            var evt = Mock.Event();
            var expected = evt.Category;

            var changeCategoryAction = new ChangeCategoryActionHandler();

			changeCategoryAction.ApplyAsync(
				evt,
				new ActionDefinition()
				{
					Properties = new[]
					{
						new Property("some-unrelated-property", "some unrelated value")
					}
				},
				new Rule() { Name = "Mocked Rule" }).Wait();

			var actual = evt.Category;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Action_ChangeCategory_NullArgs()
        {
            var evt = Mock.Event();
            var expected = evt.Category;

            var changeCategoryAction = new ChangeCategoryActionHandler();

			changeCategoryAction.ApplyAsync(
				evt,
				new ActionDefinition()
				{
					Properties = null
				},
				new Rule() { Name = "Mocked Rule" }).Wait();

			var actual = evt.Category;

            Assert.AreEqual(expected, actual);
        }
    }
}
