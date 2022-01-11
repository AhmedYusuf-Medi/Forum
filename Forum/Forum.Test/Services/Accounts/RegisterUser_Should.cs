using Forum.Data;
using Forum.Models.Request.User;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Accounts
{
    [TestClass]
    public class RegisterUser_Should : BaseTest
    {
        [TestMethod]
        public async Task Return_Correct_Massage_When_Email_Exist()
        {
            var UserForRegister = new UserRegisterRequestModel() 
            { 
                Email = "stevenvselenski@gmail.com", 
                UserName = "stevenvselenski",
                Password = "passwordQ1!",
                DisplayName = "Steven"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.RegisterUserAsync(UserForRegister);

                Assert.AreEqual(false, actual.IsSuccess);
                Assert.AreEqual("User already exist", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_Massage_When_UserName_Exist()
        {
            var UserForRegister = new UserRegisterRequestModel()
            {
                Email = "steven@gmail.com",
                UserName = "stevenvselenski",
                Password = "passwordQ1!",
                DisplayName = "Steven"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.RegisterUserAsync(UserForRegister);

                Assert.AreEqual(false, actual.IsSuccess);
                Assert.AreEqual("User already exist", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_Massage_When_Registration_Succeed()
        {
            var UserForRegister = new UserRegisterRequestModel()
            {
                Email = "stevenvselenski_new@gmail.com",
                UserName = "stevenvselenski_new",
                Password = "passwordQ1!",
                DisplayName = "Steven"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);
               
                var actual = await sut.RegisterUserAsync(UserForRegister);

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual("Please check your email for verification link.", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_RegisterUser()
        {
            var UserForRegister = new UserRegisterRequestModel()
            {
                Email = "stevenvselenski_new@gmail.com",
                UserName = "stevenvselenski_new",
                Password = "passwordQ1!",
                DisplayName = "Steven"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.RegisterUserAsync(UserForRegister);

                var UserToLogin = new UserLoginRequestModel() { Email = "stevenvselenski_new@gmail.com", Password = "passwordQ1!" };
                var newRegisteredUser = await sut.LoginAsync(UserToLogin);

                Assert.AreEqual("stevenvselenski_new", newRegisteredUser.Payload.Username);
                Assert.AreEqual("Pending", newRegisteredUser.Payload.Role);
            }
        }
    }
}