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
    public class OrderBy_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 3, "mr", "", "", "", "", null)]
        [DataRow(2, 5, "", "mc", "", "", "", null)]
        [DataRow(1, 3, "", "", "ml", "", "", null)]
        [DataRow(1, 3, "", "", "", "ta", "", null)]
        [DataRow(1, 3, "", "", "", "", "td", null)]
        [DataRow(1, 10, "mr", "mc", "ml", "ta", "td", null)]
        [DataRow(1, 12, "mr", "mc", "ml", "ta", "td", 10)]
        public async Task OrderBy_ShouldReturnSorted_PaginatedCollection_With_PostResponseModels(int page, int perPage, 
            string mostRecently, string mostCommented,
            string mostLiked, string titleAsc, 
            string titleDes, int? top)
        {
            var requestModel = new PostSortRequestModel()
            {
                Page = page,
                PerPage = perPage,
                MostRecently = mostRecently,
                MostLiked = mostLiked,
                MostCommented = mostCommented,
                TitleAsc = titleAsc,
                TitleDes =titleDes,
                Top = top
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.OrderByAsync(requestModel);

                if (requestModel.Top.HasValue)
                {
                    Assert.AreEqual(actual.Payload.Entities.Count(), top);
                }
                else
                {
                    Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
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