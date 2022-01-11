using Forum.Data;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Users
{
    [TestClass]
    public class DeleteUser_Should : BaseTest
    {
        [TestMethod]
        public async Task Return_correct_response()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);
                string okResponseMassage = "User was successfully deleted!";
                long userId = 2;

                var actual = await sut.DeleteAsync(userId);

                var afterDeleted = await sut.GetByIdAsync(userId);
                string afterDeleteMassage = "Doesn't exist such a User";

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual(actual.Message, okResponseMassage);

                Assert.AreEqual(false, afterDeleted.IsSuccess);
                Assert.AreEqual(afterDeleted.Message, afterDeleteMassage);
            }
        }
    }
}