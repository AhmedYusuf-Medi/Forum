using Forum.Data;
using Forum.Models.Request.Post;
using Forum.Models.Response;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class Edit_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 1, "Modified Title" ,"Modified Description")]
        [DataRow(3, 7, "Modified Title" ,"Another Modified Description")]
        public async Task Edit_Should_ModifyPostFromDatabase(long userId, long postId, string title, string description)
        {
            var requestModel = new EditPostRequestModel()
            {
                Title = title,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(postId, userId, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Post was successfully edited!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 0, "Modified Title", "Modified Description")]
        [DataRow(3, long.MaxValue, "Modified Title", "Another Modified Description")]
        public async Task Edit_Should_NotModify_BecauseOfNonExistingPost(long userId, long postId, string title, string description)
        {
            var requestModel = new EditPostRequestModel()
            {
                Title = title,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(postId, userId, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Post");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(4, 1, "Modified Title", "Modified Description")]
        [DataRow(2, 7, "Modified Title", "Another Modified Description")]
        public async Task Edit_Should_NotModify_BecauseOfNotOwningPost(long userId, long postId, string title, string description)
        {
            var requestModel = new EditPostRequestModel()
            {
                Title = title,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(postId, userId, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "You do not have the enough permission for the operation!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 1, "", "")]
        [DataRow(3, 7, null, null)]
        [DataRow(1, 1, "   ", "    ")]
        [DataRow(3, 7, " ", " ")]
        public async Task Edit_Should_NotModify_BecauseOfIncorrectRequest(long userId, long postId, string title, string description)
        {
            var requestModel = new EditPostRequestModel()
            {
                Title = title,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.EditAsync(postId, userId, requestModel);

                var post = await sut.GetByIdAsync(postId);

                Assert.AreNotEqual(post.Payload.Description, description);
                Assert.AreNotEqual(post.Payload.Title, title);
            }
        }
    }
}