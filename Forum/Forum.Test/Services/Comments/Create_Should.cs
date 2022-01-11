//Local
using Forum.Data;
using Forum.Models.Request.Comment;
using Forum.Models.Response;
using Forum.Service;
using Forum.Service.Common.Exceptions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class Create_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 1, "Description")]
        [DataRow(3, 6, "Description")]
        public async Task CreateShould_AddNewComment_ToDatabase(long userId, long postId, string description)
        {
            var requestModel = new CreateCommentRequestModel()
            {
                UserId = userId,
                PostId = postId,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Comment was successfully created!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0, 1, "Description")]
        [DataRow(1, 0, "Description")]
        [DataRow(long.MaxValue, 1, "Description")]
        [DataRow(1, long.MaxValue, "Description")]
        public async Task CreateShould_NotAddNewComment_ToDatabase(long userId, long postId, string description)
        {
            var requestModel = new CreateCommentRequestModel()
            {
                UserId = userId,
                PostId = postId,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                await Assert.ThrowsExceptionAsync<BadRequestException>(() => sut.CreateAsync(requestModel));
            }
        }
    }
}