//Local
using Forum.Models.Entities;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public class PostSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext)
        {
            if (await dbContext.Posts.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var posts = new HashSet<(long UserId, long CategoryId, string Title, string Description)>
            {
                (1,  1,"G2's new member!", "Sergen “Broken Blade” Çelik will sign with G2 Esports as their top laner ahead of the 2022 season, sources told Upcomer on Wednesday. The deal will last through the end of 2024."),
                (2,  2, "Code pasta", "I wrote some code yesterday while cooking pasta, today i realized that it become code-pasta!"),
                (1,  4, "Java", "So this .NET jedis do not need to write billion lines of code for properties?!"),
                (3,  3, "Visual Studio 2022", "I am really excited to announce that we've reached general availability for Visual Studio 2022."),
                (1,  5, "What does === in JS?", "Equality(===): This operator is used to compare the equality of two operands with type. If both value and type are equal then the condition is true otherwise false."),  
                (2,  6, "Sabaton", "Today I listened to sabaton that much that I broked my finger while coding!"),               
                (3,  7, "SpaceX", "It's good to learn new stuffs about SpaceX, I would like if somebody shared with me!"),
                (1,  8, "Lada Niva", "Yo guys, one week ago I did a offroad in the garden where had a lot of watermelons now my car doesn't want to talk with me!"),            
                (2,  9, "SPIDER-MAN: NO WAY HOME", "I bet you don't know how much happy I'm because 2021 we will have opportunity to watch this movie!"),
                (3,  10, "Alpha C# 31 Cohort", "Hi dear Friends, I would like to share with you guys that for us (Ahmed and Veselin) was a honnor to study and work with you in Telerik Academy, Thanks!"),           
                (1,  7, "CREW-2 returns to earth", "After 199 days in space, the longest-duration mission for a U.S. spacecraft, Dragon and the Crew-2 astronauts, Shane Kimbrough, Megan McArthur, Akihiko Hoshide, and Thomas Pesquet, returned to Earth, splashing down off the coast of Pensacola, Florida at 10:33 p.m. EST on November 8."),               
                (2,  4, ".NET :)", "The best platform in the earth, change my mind!"),
                (1,  1, "HYLISSANG SIGNS A 2-YEAR EXTENSION", "Ever since joining FNC in 2018, Zdravets “Hylissang” Galabov has had a fundamental impact on how our League of Legends team play the game. His aggressive, unrelenting play-style has become synonymous with FNC over the years. Signature engages on champions such as Pyke and Rakan has cemented his reputation in League of Legends E-Sports as one of the most feared supports to play against in the World. He is popularly known as “The Professor” in Korea for his creative angles and one-of-a-kind plays."),
            };

            foreach (var post in posts)
            {
                await dbContext.Posts.AddAsync(new Post() 
                { 
                    UserId = post.UserId,
                    CategoryId = post.CategoryId,
                    Title = post.Title,
                    Description = post.Description,
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}