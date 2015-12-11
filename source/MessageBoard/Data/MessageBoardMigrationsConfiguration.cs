using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MessageBoard.Data
{
    internal class MessageBoardMigrationsConfiguration : DbMigrationsConfiguration<MessageBoardContext>
    {
        public MessageBoardMigrationsConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;//maybe in debug - creaful with live settings!
            this.AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(MessageBoardContext context)
        {
            base.Seed(context);//called on every application start up - every time a new app domain is started
#if DEBUG
            if(context.Topics.Count() == 0)
            {
                var topic = new Topic()
                {
                    Title = "This is topic 1",
                    Created = DateTime.Now,
                    Body = "Hello from tpoic 1",
                    Replies = new List<Reply>()
                    {
                        new Reply()
                        {
                            Body = "Hello Topic 1",
                            Created = DateTime.Now
                        },
                        new Reply()
                        {
                            Body = "Whatever Topic 1",
                            Created = DateTime.Now
                        },
                        new Reply()
                        {
                            Body = "Goodbye Topic 1",
                            Created = DateTime.Now
                        }
                    }
                };
                context.Topics.Add(topic);
                topic = new Topic()
                {
                    Title = "This is topic 2",
                    Created = DateTime.Now,
                    Body = "Hello from topic 2",
                    Replies = new List<Reply>()
                    {
                        new Reply()
                        {
                            Body = "Hello Topic 2",
                            Created = DateTime.Now
                        },
                        new Reply()
                        {
                            Body = "Whatever Topic 2",
                            Created = DateTime.Now
                        },
                        new Reply()
                        {
                            Body = "Goodbye Topic 2",
                            Created = DateTime.Now
                        }
                    }
                };
                context.Topics.Add(topic);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }
#endif
        }
    }
}