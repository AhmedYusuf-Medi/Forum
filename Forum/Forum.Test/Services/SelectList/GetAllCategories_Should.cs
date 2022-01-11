//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.SelectList
{
    [TestClass]
    public class GetAllCategories_Should : BaseTest
    {
        [TestMethod]
        public async Task GetAllCategories_ShouldReturn_PaginatedCollection_With_CategoryResponseModels()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new SelectListService(assertContext);

                var actual = await sut.GetAllCategoriesAsync();

                Assert.IsNotNull(actual);
                CollectionAssert.AllItemsAreInstancesOfType(actual.ToList(), typeof(Category));
            }
        }
    }
}