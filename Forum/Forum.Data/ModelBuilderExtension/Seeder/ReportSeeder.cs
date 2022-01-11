//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class ReportSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Reports.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var reports= new HashSet<(long reportTypeId, long senderId, long receiverId, string description)>
            {
                (1, 1, 2, "He roast so much Java!"),
                (2, 2, 1, "He is blaming me for roasting Java!"),
                (3, 3, 1, "This comment somehow tilts me."),
                (2, 3, 2, "This comment somehow tilts me too."),
            };

            foreach (var report in reports)
            {
                await dbContext.Reports.AddAsync(new Report()
                {
                    ReportTypeId = report.reportTypeId,
                    ReceiverId = report.receiverId,
                    SenderId = report.senderId,
                    Description = report.description
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}