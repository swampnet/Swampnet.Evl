using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Client;
using Swampnet.Evl.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Mocks;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class EventsControllerTests
    {
        [TestMethod]
        public void EventsControllerTests_CreateController()
        {
            var events = new EventsController(
                Mock.EventDataAccess(), 
                Mock.EventQueueProcessor(), 
                Mock.Auth());
        }


        [TestMethod]
        public void EventsControllerTests_GetCategories()
        {
            var events = new EventsController(
                Mock.EventDataAccess(),
                Mock.EventQueueProcessor(),
                Mock.Auth());

            var rs = events.GetCategories() as OkObjectResult;

            Assert.IsNotNull(rs);
            Assert.AreEqual(200, rs.StatusCode);

            var actual = rs.Value as IEnumerable<string>;
            Assert.IsNotNull(actual);

            var expected = Enum.GetValues(typeof(EventCategory)).Cast<EventCategory>().Select(e => e.ToString()).ToArray();

            Assert.IsTrue(actual.OrderBy(x => x).SequenceEqual(expected.OrderBy(x => x)));
        }


        [TestMethod]
        public void EventsControllerTests_GetSources()
        {
            var events = new EventsController(
                Mock.EventDataAccess(),
                Mock.EventQueueProcessor(),
                Mock.Auth());

            var rs = events.GetSources().Result as OkObjectResult;

            Assert.IsNotNull(rs);
            Assert.AreEqual(200, rs.StatusCode);

            var actual = rs.Value as IEnumerable<string>;
            Assert.IsNotNull(actual);

            var expected = new[] {
                "MOCKED-SOURCE-01",
                "MOCKED-SOURCE-02"
            };

            Assert.IsTrue(actual.OrderBy(x => x).SequenceEqual(expected.OrderBy(x => x)));
        }

        
        [TestMethod]
        public void EventsControllerTests_GetTags()
        {
            var events = new EventsController(
                Mock.EventDataAccess(),
                Mock.EventQueueProcessor(),
                Mock.Auth());

            var rs = events.GetTags().Result as OkObjectResult;

            Assert.IsNotNull(rs);
            Assert.AreEqual(200, rs.StatusCode);

            var actual = rs.Value as IEnumerable<string>;
            Assert.IsNotNull(actual);

            var expected = new[] {
                "MOCKED-TAG-01",
                "MOCKED-TAG-02",
                "MOCKED-TAG-03"
            };

            Assert.IsTrue(actual.OrderBy(x => x).SequenceEqual(expected.OrderBy(x => x)));
        }


        [TestMethod]
        public void EventsControllerTests_Search()
        {
            var dal = Mock.EventDataAccess();
            var auth = Mock.Auth();

            var events = new EventsController(
                dal,
                Mock.EventQueueProcessor(),
                auth);

            var rs = events.Get(new EventSearchCriteria()).Result as OkObjectResult;

            Assert.IsNotNull(rs);
            Assert.AreEqual(200, rs.StatusCode);

            var actual = rs.Value as IEnumerable<EventSummary>;
            Assert.IsNotNull(actual);

            Assert.AreEqual(
                dal.GetTotalEventCountAsync(auth.GetOrganisationAsync(Guid.Empty).Result).Result,
                actual.Count());
        }


        /// <summary>
        /// Test we get a 200 / Ok if event exists
        /// </summary>
        [TestMethod]
        public void EventsControllerTests_GetById()
        {
            var events = new EventsController(
                Mock.EventDataAccess(),
                Mock.EventQueueProcessor(),
                Mock.Auth());

            var rs = events.Get(Guid.Parse("6B625284-DD40-4AD5-95FF-5F6AEF6C214F")).Result as OkObjectResult;

            Assert.IsNotNull(rs);
            Assert.AreEqual(200, rs.StatusCode);            
        }


        /// <summary>
        /// Test we get a 404 if event doesn't exist
        /// </summary>
        [TestMethod]
        public void EventsControllerTests_GetById_404()
        {
            var events = new EventsController(
                Mock.EventDataAccess(),
                Mock.EventQueueProcessor(),
                Mock.Auth());

            var rs = events.Get(Guid.Parse("C05CFB1A-0859-49D8-A469-97E39F12720B")).Result as NotFoundResult;

            Assert.IsNotNull(rs);
            Assert.AreEqual(404, rs.StatusCode);
        }
    }
}
