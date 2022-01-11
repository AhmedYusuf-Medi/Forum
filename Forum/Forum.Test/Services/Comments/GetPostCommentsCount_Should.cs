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
    public class GetPostCommentsCount_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(3, 2)]
        public async Task GetPostCommentsCount_ShouldReturn_PostCommentsCount(long postId, long commentCount) 
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetPostCommentsCountAsync(postId);

                Assert.IsNotNull(actual);
                Assert.AreEqual(actual, commentCount);
            }
        }
    }
}