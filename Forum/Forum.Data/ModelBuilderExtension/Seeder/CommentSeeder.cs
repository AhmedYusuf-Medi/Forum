//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class CommentSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Comments.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var comments = new HashSet<(long UserId, long PostId, string Description)>
            {
               (1, 1, "Yeah but FNC are still better also TSM are win this year!"),
               (2, 1, "Only good news for us!"),
               (2, 2, "Yes, it is but I would suggests you to start cooking less and get shreeded!"),
               (1, 2, "He is right eat and sleep less and code more!"),
               (3, 3, "Damn right that's why they call us immortals!"),
               (2, 3, "We also have disco music whenever exception is thrown!"),
               (2, 4, "Yeah I'm happy about it too!"),
               (1, 4, "Is it eatable?"),
               (2, 5, "Thanks for the info dude!"),
               (2, 5, "Java what? Script? I have never seen it before!"),
               (1, 6, "Listen it once more to broke your desk too!"),
               (3, 6, "So you played NFS in your childhood?"),
               (2, 7, "They have invented such a nice car named Tesla!"),
               (1, 8, "Give him some apples!"),
               (3, 9, "Mee too, wanna watch it together while boss is thinking we are cooding?"),
               (1, 10, "Wish you a luck in the future, homies!"),
               (2, 11, "My grandpa was there when it happend."),
               (3, 11, "That's epic bro."),
               (4, 11, "It was mine idea btw!"),
               (3, 12, "Even god cannot change your mid bro, it is not possible."),
               (2, 12, "If someone trie to change your mind, beat him."),
               (2, 13, "If the support is your save you guys are loosers."),
            };

            foreach (var comment in comments)
            {
                await dbContext.Comments.AddAsync(new Comment()
                {
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    Description = comment.Description,
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}