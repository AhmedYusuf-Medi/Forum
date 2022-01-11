//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class ReportTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.ReportTypes.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var reportTypes = new HashSet<string>
            {
                "Negative Attitude",
                "Verbal Abuse",
                "Hate Speech",
                "Offensive"
            };

            foreach (var report in reportTypes)
            {
                await dbContext.ReportTypes.AddAsync(new ReportType()
                {
                    Name = report
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}