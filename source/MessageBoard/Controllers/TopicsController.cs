using MessageBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageBoard.Controllers
{
    public class TopicsController : ApiController//verb mappers. mvc is view mappers
    {
        private IMessageBoardRepository _repo;

        public TopicsController(IMessageBoardRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<Topic> Get(bool includeReplies = false)//webapi requestparam for reply stucture not data
        {
            IQueryable<Topic> results;
            if (includeReplies)
            {
                results = _repo.GetTopicsIncludingReplies();
            }
            else
            {
                results = _repo.GetTopics();
            }
            var topics = results
                    .OrderByDescending(t => t.Created)
                    .Take(25)
                    .ToList();//iquery becomes ienum
            return topics;
        }
        public HttpResponseMessage Post([FromBody]Topic newTopic)
        {
            if (newTopic.Created == default(DateTime))
            {
                newTopic.Created = DateTime.UtcNow;
            }
            if (_repo.AddTopic(newTopic) && _repo.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Created, newTopic);//201 = 200 range = success
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);//400 = error
        }
    }
}
