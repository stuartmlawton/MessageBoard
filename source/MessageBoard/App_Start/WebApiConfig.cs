using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace MessageBoard
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //formatting outside of controller logic
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();//get first json formetter from the webapi http configuration
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//use the camelCase contract resolver
            //json and javascript camelCase friendly.
            //this does not change xml so requests that use accept: text/xml header will retain data object casing in the xml result.

            //associated route
            config.Routes.MapHttpRoute(
                name: "RepliesRoute",
                routeTemplate: "api/topics/{topicid}/replies/{id}",
                defaults: new { controller = "replies", id = RouteParameter.Optional }//repliesid is NOT optional
                );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/topics/{id}",
                defaults: new { controller = "topics", id = RouteParameter.Optional }
                );
        }
    }
}
