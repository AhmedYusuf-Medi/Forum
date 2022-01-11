//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class Comment_LikeSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Comment_Likes.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var commentLikes = new HashSet<(long CommentId, long UserId)>
            {
                (1, 2),
                (2, 1),
                (3, 3),
                (4, 2),
                (5, 1),
                (6, 3),
                (7, 3),
                (8, 2),
                (9, 1),
                (10, 2),
            };

            foreach (var commentLike in commentLikes)
            {
                await dbContext.Comment_Likes.AddAsync(new Comment_Like() 
                {
                    UserId = commentLike.UserId,
                    CommentId = commentLike.CommentId,
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}