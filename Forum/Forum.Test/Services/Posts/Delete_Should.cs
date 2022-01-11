//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class Delete_Should : BaseTest
    {
        [TestMethod]
        [DataRow((long)1, 1)]
        [DataRow((long)3, 7)]
        [DataRow(null, 7)]
        public async Task Delete_Should_RemovePost_FromDatabase(long? userId, long postId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.DeleteAsync(postId, userId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Post was successfully deleted!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(3, long.MaxValue)]
        public async Task Delete_Should_NotRemovePost_NotExistingPost(long userId, long postId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.DeleteAsync(postId, userId);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Post");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(long.MaxValue, 5)]
        public async Task Delete_Should_NotRemoveComment_BecauseOfNotOwningPost(long userId, long postId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.DeleteAsync(postId, userId);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "You do not have the enough permission for the operation!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }
    }
}