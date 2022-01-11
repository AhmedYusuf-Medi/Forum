//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Roles.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var roles = new HashSet<string>() { "Admin", "User", "Blocked", "Pending" };

            foreach (var role in roles)
            {
                await dbContext.Roles.AddAsync(new Role
                {
                    Type = role
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}