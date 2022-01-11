//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Models.Response.Post;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class GetById_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(7)]
        public async Task GetById_ShouldReturn_CorrectPost_SelectedById(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got Post!");
                Assert.IsInstanceOfType(actual, typeof(Response<PostResponseModel>));
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(long.MaxValue)]
        public async Task GetById_ShouldNotSucceed_WhenGivenId_DoesntExist(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Payload);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Post");
                Assert.IsInstanceOfType(actual, typeof(Response<PostResponseModel>));
            }
        }
    }
}