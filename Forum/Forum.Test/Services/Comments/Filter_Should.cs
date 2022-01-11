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
    public class Filter_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 3, "mr", null, null, null, null)]
        [DataRow(2, 5, null, "ml", null, null, null)]
        [DataRow(1, 3, null, null, "dev", null, null)]
        [DataRow(2, 5, null, null, null, (long)1, null)]
        [DataRow(1, 3, null, null, null, null, (long)2)]
        [DataRow(2, 5, "mr", "ml", "a", (long)1, (long)2)]
        public async Task Filter_ShouldReturnSorted_PaginatedCollection_With_CommentResponseModels(int page, int perPage, 
          string mostRecently, string mostLiked, string description,
          long? userId, long? postId)
        {
            var requestModel = new CommentFilterRequestModel()
            {
                Page = page,
                PerPage = perPage,
                MostRecently = mostRecently,
                MostLiked = mostLiked,
                PostId = postId,
                UserId =  userId,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.FilterAsync(requestModel);

                if (requestModel.PostId.HasValue)
                {
                    foreach (var comment in actual.Payload.Entities)
                    {
                        Assert.AreEqual(comment.PostId, postId);
                    }
                }

                if (requestModel.UserId.HasValue)
                {
                    foreach (var comment in actual.Payload.Entities)
                    {
                        Assert.AreEqual(comment.UserId, userId);
                    }
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