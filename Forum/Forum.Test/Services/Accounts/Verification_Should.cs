using Forum.Data;
using Forum.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Forum.Test.Services.Accounts
{
    [TestClass]
    public class Verification_Should : BaseTest
    {
      
        [TestMethod]
        public async Task Return_Correct_Massage_When_Succeed()
        {
            
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);
                var code = new Guid("5C60F693-BEF5-E011-A485-80EE7300C695");
                var email = "verificationTest@gmail.com";

                var actual = await sut.VerificationAsync(email, code);

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual($"{email} was verified!", actual.Message);
            }
        }

        [TestMethod]
        public async Task Return_Correct_Massage_When_User_Was_Verified()
        {

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new AccountService(assertContext, this.CloudinaryService, this.MailService);
                var code = new Guid("5C60F693-BEF5-E011-A485-80EE7300C695");
                var email = "stevenvselenski@gmail.com";

                var actual = await sut.VerificationAsync(email, code);

                Assert.AreEqual(true, actual.IsSuccess);
                Assert.AreEqual($"Already verified!", actual.Message);
            }
        }
    }
}
