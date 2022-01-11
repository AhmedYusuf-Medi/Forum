using Forum.Data;
using Forum.Models.Request.PostLike;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.PostLikes
{
    [TestClass]
    public class PostLikes_Should : BaseTest
    {
        [TestMethod]
        public async Task Return_correct_response_when_created()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostLikeService(assertContext);
                string okResponseMassage = "Post like was successfully created!";

                long userId = 2;
                long postId = 4;

                var model = new PostLikeRequestModel() { UserId = userId, PostId = postId };
                var actual = await sut.CreateAsync(model);

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual(actual.Message, okResponseMassage);
            }
        }

        [TestMethod]
        public async Task Return_correct_response_when_deleted()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostLikeService(assertContext);
                string okResponseMassage = "Post like was successfully deleted!";

                long postLikeId = 2;


                var actual = await sut.DeleteAsync(postLikeId);

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual(actual.Message, okResponseMassage);


                var model = new PostLikeRequestModel() { UserId = 2, PostId = 2 };
                var unDeletePostlike = await sut.CreateAsync(model);
                string unDeleteMassage = "Post like was successfully created!";

                Assert.AreEqual(true, unDeletePostlike.IsSuccess);
                Assert.AreEqual(unDeletePostlike.Message, unDeleteMassage);
            }
        }
    }
}