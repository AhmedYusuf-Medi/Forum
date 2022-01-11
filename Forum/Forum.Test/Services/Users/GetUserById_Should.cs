using Forum.Data;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Users
{
    [TestClass]
    public class GetUserById_Should : BaseTest
    {
        [TestMethod]
        public async Task Return_User_with_correct_Id()
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new UserService(assertContext);
                string okResponseMassage = "Successfully got User!";
                long userId = 2;

                var actual = await sut.GetByIdAsync(userId);
                
                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual(actual.Message, okResponseMassage);

                Assert.AreEqual(userId, actual.Payload.Id);
            }
        }
    }
}