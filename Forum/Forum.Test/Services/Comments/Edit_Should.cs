//Local
using Forum.Data;
using Forum.Models.Request.Comment;
using Forum.Models.Response;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class Edit_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 1 ,"Modified Description")]
        [DataRow(2, 2 ,"Another Modified Description")]
        public async Task Edit_Should_ModifyCommentFromDatabase(long commentId, long userId, string description)
        {
            var requestModel = new EditCommentRequestModel()
            {
                Description =  description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(commentId, userId, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Comment was successfully edited!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0, 1, "Modified Description")]
        [DataRow(long.MaxValue, 2, "Another Modified Description")]
        public async Task Edit_Should_NotModifyCommentFromDatabase_BecauseOfNonExistingComment(long commentId, long userId, string description)
        {
            var requestModel = new EditCommentRequestModel()
            {
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(commentId, userId, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Comment");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 3, "Modified Description")]
        [DataRow(2, 4, "Another Modified Description")]
        public async Task Edit_Should_NotModifyCommentFromDatabase_BecauseOfNotOwningComment(long commentId, long userId, string description)
        {
            var requestModel = new EditCommentRequestModel()
            {
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(commentId, userId, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "You do not have the enough permission for the operation!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 1, "")]
        [DataRow(1, 1, "   ")]
        [DataRow(2, 1, null)]
        [DataRow(2, 1, " ")]
        public async Task Edit_Should_NotModifyCommentFromDatabase_BecauseOfIncorrectRequest(long commentId, long userId, string description)
        {
            var requestModel = new EditCommentRequestModel()
            {
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(commentId, userId, requestModel);

                var comment = await sut.GetByIdAsync(commentId);

                Assert.AreNotEqual(comment.Payload.Description, description);
            }
        }
    }
}