//Local
using Forum.Data.ModelBuilderExtension;
using Forum.Models.Entities;
using Forum.Models.Entities.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Forum.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options)
           : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Post_Like> Post_Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Comment_Like> Comment_Likes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<PostReport> PostReports { get; set; }
        public DbSet<CommentReport> CommentReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplySoftDeleteQuery();

            modelBuilder.DisableCascadeDelete();
        }

        public override int SaveChanges()
        {
            ApplyEntityRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                  CancellationToken cancellationToken = default(CancellationToken))
        {
            ApplyEntityRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyEntityRules()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.CurrentValues["CreatedOn"] = DateTime.UtcNow;
                    entry.CurrentValues["IsDeleted"] = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    entry.CurrentValues["DeletedOn"] = DateTime.UtcNow;
                }
            }
        }

        public void Undelete(IEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }
    }
}