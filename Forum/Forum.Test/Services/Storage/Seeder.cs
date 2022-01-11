//Local
using Forum.Data;
using Forum.Models.Entities;
//Public
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Test.Services.Storage
{
    public class Seeder
    {
        private static long id = 0;

        public static async Task SeedAsync(ForumDbContext dbContext)
        {
            await dbContext.Roles.AddRangeAsync(SeedRoles());
            await dbContext.Users.AddRangeAsync(SeedUsers());
            await dbContext.Categories.AddRangeAsync(SeedCatgories());
            await dbContext.Posts.AddRangeAsync(SeedPosts());
            await dbContext.Post_Likes.AddRangeAsync(SeedPostLikes());
            await dbContext.Comments.AddRangeAsync(SeedComments());
            await dbContext.Comment_Likes.AddRangeAsync(SeedCommentLikes());
            await dbContext.ReportTypes.AddRangeAsync(SeedReportTypes());
            await dbContext.Reports.AddRangeAsync(SeedReports());
            await dbContext.PostReports.AddRangeAsync(SeedPostReports());
            await dbContext.CommentReports.AddRangeAsync(SeedCommentReports());
        }

        public static void Seed(ForumDbContext dbContext)
        {
            dbContext.Roles.AddRange(SeedRoles());
            dbContext.Users.AddRange(SeedUsers());
            dbContext.Categories.AddRange(SeedCatgories());
            dbContext.Posts.AddRange(SeedPosts());
            dbContext.Post_Likes.AddRange(SeedPostLikes());
            dbContext.Comments.AddRange(SeedComments());
            dbContext.Comment_Likes.AddRange(SeedCommentLikes());
        }

        private static IEnumerable<Role> SeedRoles()
        {
            var roleNames = new HashSet<string>() { "Admin", "User", "Blocked", "Pending" };

            var roles = new HashSet<Role>();

            id = 0;

            foreach (var role in roleNames)
            {
                roles.Add(new Role() { Id = ++id, Type = role });
            }

            return roles;
        }

        private static IEnumerable<User> SeedUsers()
        {
            var usersParamters = new List<(string Email, string Password,
             string Username, string Dspname,
             long RoleId, string picturePath)>
            {
                ("stevenvselenski@gmail.com", "passwordQ1!", "stevenvselenski", "Steven", 2, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1637483275/kteur8mwob5vf4csta1x_ztj672.png"),
                ("muthkabarona@gmail.com", "passwordQ1!", "muthkabarona", "Ahmed", 1, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1637483275/kteur8mwob5vf4csta1x_ztj672.png"),
                ("veskotech@test.com", "passwordQ1!", "vesko.t", "Veselin", 1, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1637483275/kteur8mwob5vf4csta1x_ztj672.png"),
                ("verificationTest@gmail.com", "passwordQ1!", "steven_verification", "Steven", 4, "https://res.cloudinary.com/ddipdwbtm/image/upload/v1637483275/kteur8mwob5vf4csta1x_ztj672.png")
            };

            id = 0;

            var users = new HashSet<User>();

            id = 0;

            foreach (var user in usersParamters)
            {
                users.Add(new User
                {
                    Id= ++id,
                    Email = user.Email,
                    Password = user.Password,
                    Username = user.Username,
                    DisplayName = user.Dspname,
                    RoleId = user.RoleId,
                    PicturePath = user.picturePath,
                    Code = new Guid("5C60F693-BEF5-E011-A485-80EE7300C695")
                });
            }

            return users;
        }

        private static IEnumerable<Category> SeedCatgories()
        {
            var categoryNames = new HashSet<string>()
            {
                "Sport",
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

            var categories = new HashSet<Category>();

            id = 0;

            foreach (var category in categoryNames)
            {
                categories.Add(new Category() { Id = ++id, Name = category });
            }

            return categories;
        }

        private static IEnumerable<Post> SeedPosts()
        {
            var postParamters = new HashSet<(long UserId, long CategoryId, string Title, string Description)>
            {
                (1,  1,"Ronaldo","Ronaldo losed his hair but he refreshed it with NiveYa shampoo!"),
                (2,  2, "Code pasta","I wrote some code yesterday while cooking pasta, today i realized that it become code-pasta!"),
                (3,  3, "Java :(","As a .NET Devolper I am gonna do my job and tradition to tell Java developers that java sucks!"),
                (1,  4, ".NET :(","I can't get it why do this .NET Developers roast us alway!"),
                (1,  5, "JavaScript is Monkey language","I'm not sure but JS looks like a gorila to me strong but lazy!"),
                (2,  6, "Sabaton","Today I listened to sabaton that much that I broked my finger while coding!"),
                (3,  7, "SpaceX","It's good to learn new stuffs about SpaceX, I would like if somebody shared with me!"),
                (1,  8, "Lada Niva","Yo guys, one week ago I did a offroad in the garden where had a lot of watermelons now my car doesn't want to talk with me!"),
                (2,  9, "SPIDER-MAN: NO WAY HOME","I bet you don't know how much happy I'm because 2021 we will have opportunity to watch this movie!"),
                (3,  10, "Alpha C# 31 Cohort","Hi dear Friends, I would like to share with you guys that for us (Ahmed and Veselin) was a honnor to study and work with you in Telerik Academy, Thanks!"),
                (1,  7, "CREW-2 returns to earth","After 199 days in space, the longest-duration mission for a U.S. spacecraft, Dragon and the Crew-2 astronauts, Shane Kimbrough, Megan McArthur, Akihiko Hoshide, and Thomas Pesquet, returned to Earth, splashing down off the coast of Pensacola, Florida at 10:33 p.m. EST on November 8."),
                (2,  4, ".NET :)","The best platform in the earth, change my mind!"),
            };

            var posts = new HashSet<Post>();

            id = 0;

            foreach (var post in postParamters)
            {
                posts.Add(new Post()
                {
                    Id=++id,
                    UserId = post.UserId,
                    CategoryId = post.CategoryId,
                    Title = post.Title,
                    Description = post.Description,
                });
            }

            return posts;
        }

        private static IEnumerable<Post_Like> SeedPostLikes()
        {
            var postLikesParameters = new HashSet<(long PostId, long UserId)>
            {
                (1,3),
                (2,2),
                (3,1),
                (4,1),
                (5,2),
                (6,3),
                (7,3),
                (8,2),
                (9,1),
                (10,3),
                (1,2),
                (2,1),
                (3,3),
                (3,2),
            };

            var postLikes = new HashSet<Post_Like>();

            id = 0;

            foreach (var postLike in postLikesParameters)
            {
                postLikes.Add(new Post_Like
                {
                    Id = ++id,
                    UserId = postLike.UserId,
                    PostId = postLike.PostId
                });
            }

            return postLikes;
        }

        private static IEnumerable<Comment> SeedComments()
        {
            var commentsParameters = new HashSet<(long UserId, long PostId, string Description)>
            {
               (1,  1, "Yeah, it is a miracle how one shampoo can recover his hair!"),
               (2,  1, "Ha-ha, that was a  nice joke!"),
               (2,  2, "Yes, it is but I would suggests you to start cooking less and get shreeded!"),
               (1,  2, "He is right eat and sleep less and code more!"),
               (3,  3, "Yeah, this guys they are such a losers!"),
               (2,  3, "Leave them to rest dude, they should write extra code because they are not using .NET!"),
               (3,  4, "You deserve it!"),
               (2,  5, "I guess they are a monkeys!"),
               (1,  6, "Listen it once more to broke your desk too!"),
               (2,  7, "They have invented such a nice car named Tesla!"),
               (1,  8, "Give him some apples!"),
               (3,  9, "Mee too, wanna watch it together while boss is thinking we are cooding?"),
               (1,  10, "Wish you a luck in the future, homies!"),
            };

            var comments = new HashSet<Comment>();

            id = 0;

            foreach (var comment in commentsParameters)
            {
                comments.Add(new Comment()
                {
                    Id = ++id,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    Description = comment.Description,
                });
            }

            return comments;
        }

        private static IEnumerable<Comment_Like> SeedCommentLikes()
        {
            var commentLikesParameters = new HashSet<(long CommentId, long UserId)>
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

            var commentLikes = new HashSet<Comment_Like>();

            id = 0;

            foreach (var postLike in commentLikesParameters)
            {
                commentLikes.Add(new Comment_Like
                {
                    Id = ++id,
                    UserId = postLike.UserId,
                    CommentId = postLike.CommentId
                });
            }

            return commentLikes;
        }

        private static IEnumerable<ReportType> SeedReportTypes()
        {
            var reportTypeParamters = new HashSet<string>
            {
                "Negative Attitude",
                "Verbal Abuse",
                "Hate Speech",
                "Offensive"
            };

            var reportTypes = new HashSet<ReportType>();

            id = 0;

            foreach (var report in reportTypeParamters)
            {
                reportTypes.Add(new ReportType
                {
                    Id = ++id,
                    Name = report
                });
            }

            return reportTypes;
        }

        private static IEnumerable<Report> SeedReports()
        {
            var reportParamters = new HashSet<(long reportTypeId, long senderId, long receiverId, string description)>
            {
                (1,1,2, "He roast so much Java!"),
                (2,2,1, "He is blaming me for roasting Java!"),
                (3, 3, 1, "This comment somehow tilts me."),
                (2, 3, 2, "This comment somehow tilts me too."),
            };

            var reports = new HashSet<Report>();

            id = 0;

            foreach (var report in reportParamters)
            {
                reports.Add(new Report()
                {
                    ReportTypeId = report.reportTypeId,
                    ReceiverId = report.receiverId,
                    SenderId = report.senderId,
                    Description = report.description
                });
            }

            return reports;
        }

        private static IEnumerable<PostReport> SeedPostReports()
        {
            var postReportParamters = new HashSet<(long postId, long reportId)>
            {
                (1, 1)
            };

            var postReports = new HashSet<PostReport>();

            id = 0;

            foreach (var report in postReportParamters)
            {
                postReports.Add(new PostReport
                {
                    PostId = report.postId,
                    ReportId = report.reportId
                });
            }

            return postReports;
        }

        private static IEnumerable<CommentReport> SeedCommentReports()
        {
            var commentReportParameters = new HashSet<(long commentId, long reportId)>
            {
              (3, 3),
              (4, 4)
            };

            var postReports = new HashSet<CommentReport>();

            id = 0;

            foreach (var report in commentReportParameters)
            {
                postReports.Add(new CommentReport
                {
                    CommentId = report.commentId,
                    ReportId = report.reportId
                });
            }

            return postReports;
        }
    }
}