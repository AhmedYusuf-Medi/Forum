using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class ForumDbContextSeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            var seeders = new List<ISeeder>
                          {
                              new RoleSeeder(),
                              new UserSeeder(),
                              new CategorySeeder(),
                              new ReportTypeSeeder(),
                              new ReportSeeder(),
                              new PostSeeder(),
                              new PostReportSeeder(),
                              new Post_LikeSeeder(),
                              new CommentSeeder(),
                              new CommentReportSeeder(),
                              new Comment_LikeSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext);
            }
        }
    }
}