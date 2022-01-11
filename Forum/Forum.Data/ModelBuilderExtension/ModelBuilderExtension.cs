//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Linq;

namespace Forum.Data.ModelBuilderExtension
{
    public static class ModelBuilderExtension
    {
        public static void DisableCascadeDelete(this ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                                                .ToList();

            var foreignKeys = entityTypes.SelectMany(e => e.GetForeignKeys()
                                         .Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public static void ApplySoftDeleteQuery(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => EF.Property<bool>(u, "IsDeleted") == false);
            modelBuilder.Entity<Role>().HasQueryFilter(r => EF.Property<bool>(r, "IsDeleted") == false);
            modelBuilder.Entity<Post>().HasQueryFilter(p => EF.Property<bool>(p, "IsDeleted") == false);
            modelBuilder.Entity<Post_Like>().HasQueryFilter(pl => EF.Property<bool>(pl, "IsDeleted") == false);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
            modelBuilder.Entity<Comment_Like>().HasQueryFilter(cl => EF.Property<bool>(cl, "IsDeleted") == false);
            modelBuilder.Entity<Category>().HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
            modelBuilder.Entity<ReportType>().HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
            modelBuilder.Entity<Report>().HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
            modelBuilder.Entity<PostReport>().HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
            modelBuilder.Entity<CommentReport>().HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
        }
    }
}