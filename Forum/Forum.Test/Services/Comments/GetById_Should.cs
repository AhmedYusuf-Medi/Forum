//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Models.Response.Comment;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class GetById_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(7)]
        public async Task GetById_ShouldReturn_CorrectComment_SelectedById(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got Comment!");
                Assert.IsInstanceOfType(actual, typeof(Response<CommentResponseModel>));
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(long.MaxValue)]
        public async Task GetById_ShouldNotSucceed_WhenGivenId_DoesntExist(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Payload);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Comment");
                Assert.IsInstanceOfType(actual, typeof(Response<CommentResponseModel>));
            }
        }
    }
}