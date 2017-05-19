using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;
using GreentubeGameTask.Infrastructure.EntityConfigurations;

namespace GreentubeGameTask.Infrastructure
{
    public class UserGameContext : DbContext
    {
        public UserGameContext()
            : base("name=UserGameContext")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<UserGameCommentRate> UserGameCommentsRates { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            var context = ((IObjectContextAdapter) this).ObjectContext;
            foreach (
                var entity in
                    context.ObjectStateManager.GetObjectStateEntries(EntityState.Added)
                        .Select(entry => entry.Entity)
                        .OfType<ICreatedOn>())
            {
                entity.CreatedOn = DateTime.Now;
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GameEntityConfiguration());
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new UserGameCommentRateEntityConfiguration());
        }
    }
}