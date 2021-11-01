using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dgk.Application.Interface;
using Dgk.Domain.Attributes;
using Dgk.Domain.User;

namespace Dgk.Persistence.Context
{
    public class DataBaseContext : DbContext,IDataBaseContext
    {
        public DbSet<User> Users { get; set; }

        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute),true).Length>0)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("InsertTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemove");
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified ||
                            p.State == EntityState.Added ||
                            p.State == EntityState.Deleted);

            foreach (var item in modifiedEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());

                var inserted = entityType.FindProperty("InsertTime");
                var updated = entityType.FindProperty("UpdateTime");
                var removeTime = entityType.FindProperty("RemoveTime");
                var removed = entityType.FindProperty("IsRemove");

                if (item.State==EntityState.Added && inserted != null)
                {
                    item.Property("InsertTime").CurrentValue = DateTime.Now;
                }

                if (item.State == EntityState.Modified && updated != null)
                {
                    item.Property("UpdateTime").CurrentValue = DateTime.Now;
                }

                if (item.State == EntityState.Deleted && removeTime != null && removed != null)
                {
                    item.Property("RemoveTime").CurrentValue = DateTime.Now;
                    item.Property("IsRemove").CurrentValue = true;
                }
            }

            return base.SaveChanges();
        }
    }
}
