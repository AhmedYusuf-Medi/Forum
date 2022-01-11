//Local
using Forum.Data;
using Forum.Models.Request.Comment;
using Forum.Models.Response.Comment;
using Forum.Service;
using Forum.Service.Common.Extensions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class OrderBy_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 3, "mr", "", null)]
        [DataRow(2, 5, "", "ml", null)]
        [DataRow(1, 3, "mr", "ml", null)]
        [DataRow(1, 3, "", "", (long)1)]
        [DataRow(1, 3, "ml", "mr", (long)1)]
        public async Task OrderBy_ShouldReturnSorted_PaginatedCollection_With_CommentResponseModels(int page, int perPage, string mostRecently, 
            string mostLiked, long? postId)
        {
            var requestModel = new CommentSortRequestModel()
            {
                Page = page,
                PerPage = perPage,
                MostRecently = mostRecently,
                MostLiked = mostLiked,
                PostId = postId
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.OrderByAsync(requestModel);

                if (requestModel.PostId.HasValue)
                {
                    foreach (var comment in actual.Payload.Entities)
                    {
                        Assert.AreEqual(comment.PostId, postId);
                    }
                }
                else
                {
                    Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
                }

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all comments!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<CommentResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(CommentResponseModel));
            }
        }
    }
}