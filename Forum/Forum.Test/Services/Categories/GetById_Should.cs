//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Models.Response.Category;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Categories
{
    [TestClass]
    public class GetById_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(6)]
        [DataRow(9)]
        public async Task GetById_ShouldReturn_CorrectCategory_SelectedById(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload);
                Assert.AreEqual(actual.Payload.Id, id);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got Category!");
                Assert.IsInstanceOfType(actual, typeof(Response<CategoryResponseModel>));
            }
        }

        [TestMethod]
        [DataRow(long.MaxValue)]
        [DataRow(0)]
        public async Task GetById_ShouldNotSucceed_WhenGivenId_DoesntExist(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Payload);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Category");
                Assert.IsInstanceOfType(actual, typeof(Response<CategoryResponseModel>));
            }
        }
    }
}