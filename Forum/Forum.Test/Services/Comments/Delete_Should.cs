//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class Delete_Should : BaseTest
    {
        [TestMethod]
        [DataRow((long)1, 1)]
        [DataRow((long)3, 5)]
        [DataRow(null, 5)]
        public async Task Delete_Should_RemoveComment_FromDatabase(long? userId, long commentId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.DeleteAsync(commentId, userId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Comment was successfully deleted!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(3, long.MaxValue)]
        public async Task Delete_Should_NotRemoveComment_BecauseOfNotExistingComment(long userId, long commentId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.DeleteAsync(commentId, userId);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Comment");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(long.MaxValue, 5)]
        public async Task Delete_Should_NotRemoveComment_BecauseOfNotOwningComment(long userId, long commentId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.DeleteAsync(commentId, userId);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "You do not have the enough permission for the operation!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }
    }
}