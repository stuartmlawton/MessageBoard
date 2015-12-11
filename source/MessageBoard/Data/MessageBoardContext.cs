using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MessageBoard.Data
{
    public class MessageBoardContext : DbContext
    {
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Reply> Replies { get; set; }

        public MessageBoardContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;//dont preload linked objects by default
            this.Configuration.ProxyCreationEnabled = false;
            //Database.SetInitializer(
            //    new MigrateDatabaseToLatestVersion<MessageBoardContext, MessageBoardMigrationsConfiguration>()
            //    );
        }
    }
}