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
    public class GetCount_Should : BaseTest
    {
        [TestMethod]
        public async Task GetCount_ShouldReturn_CorrectNumber()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.GetCountAsync();

                var expectedCount = 12;

                Assert.IsNotNull(actual);
                Assert.AreEqual(expectedCount, actual);
            }
        }
    }
}