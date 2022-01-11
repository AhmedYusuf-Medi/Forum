using Forum.Data;
using Forum.Models.Request.User;
using Forum.Models.Response.User;
using Forum.Service;
using Forum.Service.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Users
{
    [TestClass]
    public class SearchBy_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 3, "med", null, null)]
        [DataRow(2, 2, null, "gmail", null)]
        [DataRow(1, 3, null, null, "ah")]
        [DataRow(1, 4, "med", "gmail", "ah")]
        public async Task SearchBy_ShouldReturn_Paginated_And_FilteredUserCollection(int page, int perPage,
            string username, string email, string displayName)
        {
            var requestModel = new UserSearchRequestModel
            {
                Page = page,
                PerPage = perPage,
                Username = username,
                Email = email,
                DisplayName = displayName
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);

                var actual = await sut.SearchByAsync(requestModel);

                if (!string.IsNullOrEmpty(requestModel.Username))
                {
                    foreach (var user in actual.Payload.Entities)
                    {
                        Assert.IsTrue(user.UserName.Contains(username));
                    }
                }

                if (!string.IsNullOrEmpty(requestModel.DisplayName))
                {
                    foreach (var user in actual.Payload.Entities)
                    {
                        Assert.IsTrue(user.DisplayName.Contains(displayName));
                    }
                }

                if (!string.IsNullOrEmpty(requestModel.Email))
                {
                    foreach (var user in actual.Payload.Entities)
                    {
                        Assert.IsTrue(user.Email.Contains(email));
                    }
                }

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all users!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<UserResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(UserResponseModel));
            }
        }
    }
}