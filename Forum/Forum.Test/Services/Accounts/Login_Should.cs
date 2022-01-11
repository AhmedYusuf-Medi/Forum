using Forum.Data;
using Forum.Models.Request.User;
using Forum.Models.Response.User;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Accounts
{
    [TestClass]
    public class Login_Should : BaseTest
    {

        [TestMethod]
        public async Task Return_Correct_User_When_Exist()
        {
            var UserToLogin = new UserLoginRequestModel() { Email = "stevenvselenski@gmail.com", Password = "passwordQ1!" };

            var expected = new UserLoginResponseModel()
            { 
                Id = 1,
                Username = "stevenvselenski",
                Role = "User",
                Avatar = "https://res.cloudinary.com/ddipdwbtm/image/upload/v1637483275/kteur8mwob5vf4csta1x_ztj672.png"
            };
           
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.LoginAsync(UserToLogin);

                Assert.AreEqual(expected.Username, actual.Payload.Username);
                Assert.AreEqual(expected.Id, actual.Payload.Id);
                Assert.AreEqual(expected.Role, actual.Payload.Role);
                Assert.AreEqual(expected.Avatar, actual.Payload.Avatar);

            }
        }
        [TestMethod]
        public async Task Return_Correct_Massage_When_User_Exist()
        {
            var UserToLogin = new UserLoginRequestModel() { Email = "stevenvselenski@gmail.com", Password = "passwordQ1!" };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.LoginAsync(UserToLogin);
                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual("Successfully loged in!", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_Massage_When_User_NotExist()
        {
            var UserToLogin = new UserLoginRequestModel() { Email = "stevenvs@gmail.com", Password = "passwordQ1!" };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.LoginAsync(UserToLogin);
                Assert.AreEqual(false, actual.IsSuccess);
                Assert.AreEqual("Doesn't exist such a User", actual.Message);
            }
        }
    }
}
