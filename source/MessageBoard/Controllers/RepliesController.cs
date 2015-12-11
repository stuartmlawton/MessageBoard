using MessageBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageBoard.Controllers
{
    public class RepliesController : ApiController
    {
        IMessageBoardRepository _repo;
        public RepliesController(IMessageBoardRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<Reply> Get(int topicId)
        {
            return _repo.GetRepliesByTopic(topicId);
        }
        public HttpResponseMessage Post(int topicId, [FromBody]Reply newReply)//will get topicid from uri, reply from body
        {
            if (newReply.Created == default(DateTime))
            {
                newReply.Created = DateTime.UtcNow;
            }
            newReply.TopicId = topicId;//belts and buckles for this property
            if (_repo.AddReply(newReply) && _repo.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Created, newReply);//201 = 200 range = success
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);//400 = error

        }
    }
}
