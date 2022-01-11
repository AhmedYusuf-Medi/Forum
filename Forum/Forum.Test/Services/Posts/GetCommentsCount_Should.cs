//Local
using Forum.Data;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class GetCommentsCount_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(6, 1)]
        public async Task GetCommentsCount_ShouldReturn_CorrectNumber(long postId, long expectedCount)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.GetCommentsCountAsync(postId);

                Assert.IsNotNull(actual);
                Assert.AreEqual(expectedCount, actual);
            }
        }
    }
}