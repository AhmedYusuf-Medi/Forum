//Local
using Forum.Data;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class GetLikesCount_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(3, 1)]
        public async Task GetLikesCount_Should_CommentLikesCount(long commentId, long likesCount)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetLikesCountAsync(commentId);

                Assert.IsNotNull(actual);
                Assert.AreEqual(actual, likesCount);
            }
        }
    }
}