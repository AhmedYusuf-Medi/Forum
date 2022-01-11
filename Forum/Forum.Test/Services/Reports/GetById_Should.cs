//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Models.Response.Report;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Reports
{
    [TestClass]
    public class GetById_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task GetById_ShouldReturn_CorrectReport_SelectedById(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got Report!");
                Assert.IsInstanceOfType(actual, typeof(Response<ReportResponseModel>));
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(long.MaxValue)]
        public async Task GetById_ShouldNotSucceed_WhenGivenId_DoesntExist(long id)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.GetByIdAsync(id);

                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Payload);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Report");
                Assert.IsInstanceOfType(actual, typeof(Response<ReportResponseModel>));
            }
        }
    }
}