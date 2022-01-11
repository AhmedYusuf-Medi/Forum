//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Categories.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var categories = new HashSet<string>
            {
                "E-Sport",
                "IT",
                ".NET",
                "Java",
                "JavaScript",
                "Music",
                "SpaceX",
                "Cars",
                "Movies",
                "Telerik"
            };

            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category()
                {
                    Name = category
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}