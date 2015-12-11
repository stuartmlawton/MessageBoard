using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBoard.Controllers;
using MessageBoard.Data;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;
using Newtonsoft.Json;

namespace MessageBoard.Tests.Controllers
{
    [TestClass]
    public class TopicsControllerTest
    {
        private TopicsController _ctrl;
        [TestInitialize]
        public void Init()
        {
            _ctrl = new TopicsController(new MockMessageBoardRepository());
        }

        [TestMethod]
        public void TopicsController_Get()
        {
            var results = _ctrl.Get(true);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count() > 0);
        }

        [TestMethod]
        public void TopicsController_Post()
        {

            //no better option as per time of course (pluralsight) and no better option as at time of learning (12/2015)
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:53971/api/topics");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "topics" } });
            _ctrl.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            _ctrl.Request = request;
            _ctrl.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;


            var topic = new Topic()
            {
                Id = 1,
                Title = "UnitTestTopic",
                Body = "This is the body fo the test topic"
            };
            var results = _ctrl.Post(topic);
            Assert.AreEqual(HttpStatusCode.Created, results.StatusCode);
            var json = results.Content.ReadAsStringAsync().Result;//careulf with async in unit test
            topic = JsonConvert.DeserializeObject<Topic>(json);
            Assert.IsNotNull(topic);
            Assert.IsTrue(topic.Id > 0, "Id should be greater than 0");
            Assert.IsTrue(topic.Created > DateTime.MinValue, "Created Date should be greater than min date");
        }
    }
}
