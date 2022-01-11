using Forum.Data;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Users
{
    [TestClass]
    public class User_Block_Unblock_Should : BaseTest
    {
        [TestMethod]
        public async Task Return_correct_role_when_block()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);
                long userId = 2;

                string okResponseMassage = "Successfully blocked User!";

                var actual = await sut.BlockAsync(userId);

                string expectedRole = "Blocked";
                var user = await sut.GetByIdAsync(userId);
                var actualRole = user.Payload.Role;

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual(actual.Message, okResponseMassage);
                Assert.AreEqual(actualRole, expectedRole);
            }
        }

        [TestMethod]
        public async Task Return_correct_role_when_Unblock()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);
                long userId = 2;

                string okResponseMassage = "Successfully Unblocked User!";

                var actual = await sut.UnBlockAsync(userId);

                string expectedRole = "User";
                var user = await sut.GetByIdAsync(userId);
                var actualRole = user.Payload.Role;

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual(actual.Message, okResponseMassage);
                Assert.AreEqual(actualRole, expectedRole);
            }
        }
    }
}