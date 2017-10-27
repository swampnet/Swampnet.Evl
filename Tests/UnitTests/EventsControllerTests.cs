using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Client;
using Swampnet.Evl.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Mocks;
using System.Linq;
using Swampnet.Evl.Common.Entities;

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
                Mock.Auth(Mock.MockedProfile()));
        }


        [TestMethod]
        public void EventsControllerTests_GetCategories()
        {
            var events = new EventsController(
                Mock.EventDataAccess(),
                Mock.EventQueueProcessor(),
                Mock.Auth(Mock.MockedProfile()));

            var rs = events.GetCategories().Result as OkObjectResult;

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
                Mock.Auth(Mock.MockedProfile()));

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
                Mock.Auth(Mock.MockedProfile()));

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
            var auth = Mock.Auth(Mock.MockedProfile());

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
                Mock.Auth(Mock.MockedProfile()));

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
                Mock.Auth(Mock.MockedProfile()));

            var rs = events.Get(Guid.Parse("C05CFB1A-0859-49D8-A469-97E39F12720B")).Result as NotFoundResult;

            Assert.IsNotNull(rs);

            // Returned a 404
            Assert.AreEqual(404, rs.StatusCode);
        }


        // Event source should default to organisation name
        [TestMethod]
        public void EventsControllerTests_Post_DefaultSource()
        {
            var dal = Mock.EventDataAccess();
            var q = Mock.EventQueueProcessor();
            var auth = Mock.Auth(Mock.MockedProfile());

            var events = new EventsController(dal, q, auth);

            Assert.AreEqual(0, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(0, q.Queue.Count);

            var rs = events.Post(new Event()).Result as CreatedAtRouteResult;

            Assert.IsNotNull(rs);

            // Returned CreatedAtRoot
            Assert.AreEqual(201, rs.StatusCode);

            // Add single event to dal & queue
            Assert.AreEqual(1, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(1, q.Queue.Count);

            var evt = rs.Value as EventDetails;
            
            Assert.IsNotNull(evt);

            // Source should default to organisation name
            Assert.AreEqual(auth.Profile.Organisation.Name, evt.Source);
        }


        [TestMethod]
        public void EventsControllerTests_Post_Unauthorized()
        {
            var dal = Mock.EventDataAccess();
            var q = Mock.EventQueueProcessor();
            var auth = Mock.Auth(null); // No organisation

            var events = new EventsController(dal, q, auth);

            Assert.AreEqual(0, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(0, q.Queue.Count);

            var rs = events.Post(new Event()).Result as UnauthorizedResult;

            Assert.IsNotNull(rs);
            
            // Returned unauthorized
            Assert.AreEqual(401, rs.StatusCode);

            // Didn't add or queue the event
            Assert.AreEqual(0, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(0, q.Queue.Count);
        }


        /// <summary>
        /// Posting null event should result in a bad request
        /// </summary>
        [TestMethod]
        public void EventsControllerTests_Post_Null()
        {
            var dal = Mock.EventDataAccess();
            var q = Mock.EventQueueProcessor();

            var events = new EventsController(dal, q, Mock.Auth(Mock.MockedProfile()));

            Assert.AreEqual(0, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(0, q.Queue.Count);

            var rs = events.Post(null).Result as BadRequestResult;

            Assert.IsNotNull(rs);

            // Return BadRequest
            Assert.AreEqual(400, rs.StatusCode);

            // Didn't add or queue an event
            Assert.AreEqual(0, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(0, q.Queue.Count);
        }


        [TestMethod]
        public void EventsControllerTests_Post_Bulk()
        {
            var dal = Mock.EventDataAccess();
            var q = Mock.EventQueueProcessor();

            var events = new EventsController(dal, q, Mock.Auth(Mock.MockedProfile()));

            Assert.AreEqual(0, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(0, q.Queue.Count);

            var rs = events.PostBulk(new[] 
            {
                new Event(),
                new Event(),
                new Event()
            }).Result;

            // Created & Queued 3 events
            Assert.AreEqual(3, dal.CreateCount);
            Assert.AreEqual(0, dal.UpdateCount);
            Assert.AreEqual(3, q.Queue.Count);
        }
    }
}
