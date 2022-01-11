//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class Post_LikeSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Post_Likes.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var postLikes = new HashSet<(long PostId, long UserId)>
            {
                (1, 3),
                (1, 2),
                (1, 1),
                (2, 2),
                (3, 1),
                (4, 1),
                (5, 2),
                (6, 3),
                (7, 3),
                (8, 2),
                (9, 1),
                (10, 3),
                (1, 2),
                (2, 1),
                (3, 3),
                (3, 2),
                (11, 2),
                (12, 3),
            };

            foreach (var postLike in postLikes)
            {
                await dbContext.Post_Likes.AddAsync(new Post_Like 
                {
                    UserId = postLike.UserId,
                    PostId = postLike.PostId
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}