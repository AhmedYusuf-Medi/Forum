//Local
using Forum.Data;
using Forum.Models.Request.Category;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Categories
{
    [TestClass]
    public class Edit_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, "New Name")]
        [DataRow(2, "Another new name")]
        public async Task Edit_Should_ModifyCategoryFromDatabase(long id, string name)
        {
            var requestModel = new CategoryRequestModel()
            {
                Name = name
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.EditAsync(id, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Category was successfully edited!");
            }
        }

        [TestMethod]
        [DataRow(0, "New Name")]
        [DataRow(long.MaxValue, "Another new name")]
        public async Task Edit_Should_NotModifyCategoryFromDatabase_BecauseOfNonExistingCategory(long id, string name)
        {
            var requestModel = new CategoryRequestModel()
            {
                Name = name
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.EditAsync(id, requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Category");
            }
        }

        [TestMethod]
        [DataRow(1, "")]
        [DataRow(2, " ")]
        [DataRow(2, null)]
        public async Task Edit_Should_NotModifyCategoryFromDatabase_BecauseOfIncorrectReques(long id, string name)
        {
            var requestModel = new CategoryRequestModel()
            {
                Name = name
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.EditAsync(id, requestModel);

                var category = await sut.GetByIdAsync(id);

                Assert.IsNotNull(category.Payload.Category, name);
            }
        }
    }
}