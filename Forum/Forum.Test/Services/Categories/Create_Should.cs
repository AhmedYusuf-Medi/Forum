//Local
using Forum.Data;
using Forum.Models.Request.Category;
using Forum.Models.Response;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Categories
{
    [TestClass]
    public class Create_Should : BaseTest
    {
        [TestMethod]
        [DataRow("Testing with new category")]
        [DataRow("Testing with once more")]
        public async Task CreateShould_AddNewCategory_ToDatabase(string name)
        {
            var requestModel = new CategoryRequestModel() { Name = name };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Category was successfully created!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }
    }
}