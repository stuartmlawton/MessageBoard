using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageBoard.Data
{
    public class MessageBoardRepository : IMessageBoardRepository
    {
        MessageBoardContext _ctx;
        public MessageBoardRepository(MessageBoardContext ctx)
        {
            _ctx = ctx;
        }

        public bool AddReply(Reply newReply)
        {
            try
            {
                _ctx.Replies.Add(newReply);//not in db yet - just in context
                //_ctx.SaveChanges() not ran yet
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddTopic(Topic newTopic)
        {
            try
            {
                _ctx.Topics.Add(newTopic);//not in db yet - just in context
                //_ctx.SaveChanges() not ran yet
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<Reply> GetRepliesByTopic(int topicId)
        {
            return _ctx.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<Topic> GetTopics()
        {
            var ctx = new MessageBoardContext();
            return ctx.Topics;
        }

        public IQueryable<Topic> GetTopicsIncludingReplies()
        {
            return _ctx.Topics.Include("Replies");
        }

        public bool Save()
        {
            try
            {
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                //log?
                return false;
            }
        }
    }
}