using Forum.Data;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class GetLikesCount_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(6, 1)]
        public async Task GetLikesCount_ShouldReturn_CorrectNumber(long postId, long expectedCount)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.GetLikesCountAsync(postId);

                Assert.IsNotNull(actual);
                Assert.AreEqual(expectedCount, actual);
            }
        }
    }
}