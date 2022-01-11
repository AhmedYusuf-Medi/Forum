//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Users.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var users = new HashSet<(string Email, string Password, 
                string Username, string Dspname,
                long RoleId, string picturePath)>
            {
                ("stevenvselenski@gmail.com", "passwordQ1!", "stevenvselenski", "Steven", 2, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1638532559/Default_Avatar_csjlik.png"),
                ("muthkabarona@gmail.com", "passwordQ1!", "medysun", "Ahmed", 1, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1638532559/Default_Avatar_csjlik.png"),
                ("veskotech@test.com", "passwordQ1!", "vesko.t", "Veselin", 1, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1638532559/Default_Avatar_csjlik.png"),
                ("elonmuskov@spaceEx.com", "passwordQ1!", "elon-muskinator", "Elon", 2, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1638532559/Default_Avatar_csjlik.png")
            };

            foreach (var user in users)
            {
                await dbContext.Users.AddAsync(new User
                {
                    Email = user.Email,
                    Password = user.Password,
                    Username = user.Username,
                    DisplayName = user.Dspname,
                    RoleId = user.RoleId,
                    PicturePath = user.picturePath
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}