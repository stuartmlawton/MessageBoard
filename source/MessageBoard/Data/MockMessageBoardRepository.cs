using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageBoard.Data
{
    public class MockMessageBoardRepository : IMessageBoardRepository
    {
        public bool AddReply(Reply addReply)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Reply> GetRepliesByTopin(int topicId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Topic> GetTopics()
        {
            return new List<Topic>() {
                new Topic() { Id = 1, Title = "Mock Topic 1", Created = DateTime.UtcNow, Body = "Mock Topic 1 Body" },
                new Topic() { Id = 2, Title = "Mock Topic 2", Created = DateTime.UtcNow, Body = "Mock Topic 2 Body" },
                new Topic() { Id = 3, Title = "Mock Topic 3", Created = DateTime.UtcNow, Body = "Mock Topic 3 Body" }
            }.AsQueryable();
        }

        public IQueryable<Topic> GetTopicsIncludingReplies()
        {
            return GetTopics();
        }

        bool IMessageBoardRepository.AddTopic(Topic newTopic)
        {
            return true;
        }

        IQueryable<Reply> IMessageBoardRepository.GetRepliesByTopic(int topicId)
        {
            throw new NotImplementedException();
        }

        IQueryable<Topic> IMessageBoardRepository.GetTopics()
        {
            throw new NotImplementedException();
        }

        bool IMessageBoardRepository.Save()
        {
            return true;
        }
    }
}