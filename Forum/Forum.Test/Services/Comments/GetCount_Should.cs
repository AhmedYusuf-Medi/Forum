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
    public class GetCount_Should : BaseTest
    {
        [TestMethod]
        [DataRow(13)]
        public async Task GetCount_ShouldReturn_PostCommentsCount(long commentCount)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetCountAsync();

                Assert.IsNotNull(actual);
                Assert.AreEqual(actual, commentCount);
            }
        }
    }
}