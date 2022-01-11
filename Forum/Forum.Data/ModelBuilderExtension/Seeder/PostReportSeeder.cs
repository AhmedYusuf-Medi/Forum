//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class PostReportSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.PostReports.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var postReports = new HashSet<(long postId, long reportId)>
            {
                (1, 1)
            };

            foreach (var report in postReports)
            {
                await dbContext.PostReports.AddAsync(new PostReport
                {
                    PostId = report.postId,
                    ReportId = report.reportId
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}