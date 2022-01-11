using Forum.Data;
using Forum.Models.Request.User;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Forum.Test.Services.Accounts
{
    [TestClass]
    public class EditUser_Should : BaseTest
    {

        [TestMethod]
        public async Task Return_Correct_Massage_When_User_ForEdit_Not_Exist()
        {
            long unExistId = 10;
            var user = new UserEditRequestModel()
            {
                Email = "stevenvselenski_edit@gmail.com",
                Username = "stevenvselenski_edit",
                Password = "passwordQ1_edit!",
                DisplayName = "Steven_edit"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.EditUserAsync(unExistId, user);

                Assert.AreEqual(false, actual.IsSuccess);
                Assert.AreEqual("Doesn't exist such a User", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_Massage_When_User_ForEdit_Exist()
        {
            var user = new UserEditRequestModel()
            {
                Email = "stevenvselenski_edit@gmail.com",
                Username = "stevenvselenski_edit",
                Password = "passwordQ1_edit!",
                DisplayName = "Steven_edit"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);

                var actual = await sut.EditUserAsync(1,user);

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual("User was successfully edited!", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_User_Data_After_Edit()
        {
            var user = new UserEditRequestModel()
            {
                Email = "stevenvselenski_edit@gmail.com",
                Username = "stevenvselenski_edit",
                Password = "passwordQ1_edit!",
                DisplayName = "Steven_edit"
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);
                var sut_1 = new UserService(assertContext);
                long userId = 1;

                var actual = await sut.EditUserAsync(userId, user);
                var actualAfterEdit = await sut_1.GetByIdAsync(userId);

                Assert.AreEqual(user.Email, actualAfterEdit.Payload.Email);
                Assert.AreEqual(user.Username, actualAfterEdit.Payload.UserName);
                Assert.AreEqual(user.DisplayName, actualAfterEdit.Payload.DisplayName);
                Assert.AreEqual(user.Password, actualAfterEdit.Payload.Password);
            }
        }
    }
}
