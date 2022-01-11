using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Response.User;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Users
{
    [TestClass]
    public class GetUsers_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_With_UserResponseModel(int page, int perPage)
        {
            var requestModel = new PaginationRequestModel()
            {
                Page = page,
                PerPage = perPage
            };
           
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);
               
                var actual = await sut.GetAllAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all users!");
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(UserResponseModel));
            }
        }

        [TestMethod]
        public async Task GetCount_return_correct_value()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);
                var expected = 4;
                var actual = await sut.GetCountAsync();
                
                Assert.AreEqual(expected, actual);
            }
        }
    }
}