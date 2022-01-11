using Forum.Data;
using Forum.Models.Request.CommentLike;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.CommentLikes
{
    [TestClass]
    public class CommentLikes_Should
    {
        [TestClass]
        public class PostLikes_Should : BaseTest
        {
            [TestMethod]
            public async Task Return_correct_response_when_created()
            {
                using (var assertContext = new ForumDbContext(this.Options))
                {
                    var sut = new CommentLikeService(assertContext);
                    string okResponseMassage = "Comment like was successfully created!";

                    long userId = 2;
                    long commentId = 9;

                    var model = new CommentLikeRequestModel() { UserId = userId, CommentId = commentId };
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
                    var sut = new CommentLikeService(assertContext);
                    string okResponseMassage = "Comment like was successfully deleted!";

                    long commentLikeId = 4;


                    var actual = await sut.DeleteAsync(commentLikeId);

                    Assert.AreEqual(true, actual.IsSuccess);
                    Assert.AreEqual(actual.Message, okResponseMassage);


                    var model = new CommentLikeRequestModel() { UserId = 2, CommentId = 4 };
                    var unDeleteCommentlike = await sut.CreateAsync(model);
                    string unDeleteMassage = "Comment like was successfully created!";

                    Assert.AreEqual(true, unDeleteCommentlike.IsSuccess);
                    Assert.AreEqual(unDeleteCommentlike.Message, unDeleteMassage);
                }
            }
        }
    }
}