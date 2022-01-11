//Local
using Forum.Data;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Categories
{
    [TestClass]
    public class Delete_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(7)]
        public async Task Delete_Should_RemoveCategoryFromDatabase(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.DeleteAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Category was successfully deleted!");
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(long.MaxValue)]
        public async Task Delete_Should_NotRemoveCategoryFromDatabase(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.DeleteAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Category");
            }
        }
    }
}