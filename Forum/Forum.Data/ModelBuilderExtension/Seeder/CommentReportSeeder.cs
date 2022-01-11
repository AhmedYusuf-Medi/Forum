//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class CommentReportSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.CommentReports.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var commentReports = new HashSet<(long commentId, long reportId)>
            {
                (3, 3),
                (4, 4)
            };

            foreach (var report in commentReports)
            {
                await dbContext.CommentReports.AddAsync(new CommentReport
                {
                    CommentId = report.commentId,
                    ReportId = report.reportId
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}