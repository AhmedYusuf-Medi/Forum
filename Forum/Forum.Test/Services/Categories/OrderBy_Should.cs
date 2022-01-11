//Local
using Forum.Data;
using Forum.Models.Request.Category;
using Forum.Models.Response.Category;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Categories
{
    [TestClass]
    public class OrderBy_Should : BaseTest
    {
        [TestMethod]
        [DataRow(null, "mostUploaded", 1, 3)]
        [DataRow("mostRecently", null, 2, 5)]
        [DataRow("mostRecently", "mostUploaded", 2, 5)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_With_CategoryResponseModels(string mostRecently, string mostUploaded, int page, int perPage)
        {
            var requestModel = new CategorySortRequestModel()
            {
                Page = page,
                PerPage = perPage,
                MostUploaded = mostUploaded,
                MostRecently = mostRecently
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CategoryService(assertContext);

                var actual = await sut.OrderByAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all categories!");
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(CategoryResponseModel));
            }
        }
    }
}
