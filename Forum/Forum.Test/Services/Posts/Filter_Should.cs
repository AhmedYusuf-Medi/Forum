//Local
using Forum.Data;
using Forum.Models.Request.Post;
using Forum.Models.Response.Post;
using Forum.Service;
using Forum.Service.Common.Extensions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class Filter_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 3, "mr", null, null, null, null, null, null, null, null, null, null)]
        [DataRow(1, 5, null, "ml", null, null, null, null, null, null, null, null, null)]
        [DataRow(1, 3, null, null, "mc", null, null, null, null, null, null, null, null)]
        [DataRow(1, 5, null, null, null, "dev", null, null, null, null, null, null, null)]
        [DataRow(1, 3, null, null, null, null, "net", null, null, null, null, null, null)]
        [DataRow(1, 5, null, null, null, null, null, "IT", null, null, null, null, null)]
        [DataRow(1, 3, null, null, null, null, null, null, (long)1, null, null, null, null)]
        [DataRow(1, 5, null, null, null, null, null, null, null , "ta", null, null, null)]
        [DataRow(1, 5, null, null, null, null, null, null, null, null, "td", null, null)]
        [DataRow(1, 10, null, null, null, null, null, null, null, null, null, null, 10)]
        [DataRow(1, 5, null, null, null, null, null, null, null, null, null, "stevenvselenski", null)]
        public async Task Filter_ShouldReturnSorted_PaginatedCollection_With_PostResponseModels(int page, int perPage,
          string mostRecently, string mostLiked, string mostCommented, 
          string description, string title, string category,
          long? userId, string titleAsc, string  titleDes, 
          string username, int? top)
        {
            var requestModel = new PostFilterRequestModel()
            {
                Page = page,
                PerPage = perPage,
                MostRecently = mostRecently,
                MostLiked = mostLiked,
                MostCommented =mostCommented,
                UserId = userId,
                Description = description,
                Title = title,
                Category =category,
                TitleAsc =titleAsc,
                TitleDes =titleDes,
                Username = username,
                Top = top
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.FilterAsync(requestModel);

                if (requestModel.UserId.HasValue)
                {
                    foreach (var post in actual.Payload.Entities)
                    {
                        Assert.AreEqual(post.UserId, userId);
                    }
                }

                if (requestModel.Top.HasValue)
                {
                    Assert.AreEqual(actual.Payload.Entities.Count(), top);
                }

                if (!string.IsNullOrEmpty(requestModel.Title))
                {
                    foreach (var post in actual.Payload.Entities)
                    {
                        Assert.IsTrue(post.Title.ToLower().Contains(title));
                    }
                }

                if (!string.IsNullOrEmpty(requestModel.Description))
                {
                    foreach (var post in actual.Payload.Entities)
                    {
                        Assert.IsTrue(post.Description.ToLower().Contains(description));
                    }
                }

                if (!string.IsNullOrEmpty(requestModel.Username))
                {
                    foreach (var post in actual.Payload.Entities)
                    {
                        Assert.AreEqual(post.Username, username);
                    }
                }

                if (!string.IsNullOrEmpty(requestModel.Username))
                {
                    foreach (var post in actual.Payload.Entities)
                    {
                        Assert.AreEqual(post.Username, username);
                    }
                }

                if (!string.IsNullOrEmpty(requestModel.Category))
                {
                    foreach (var post in actual.Payload.Entities)
                    {
                        Assert.AreEqual(post.Category, category);
                    }
                }

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all posts!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<PostResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(PostResponseModel));
            }
        }
    }
}