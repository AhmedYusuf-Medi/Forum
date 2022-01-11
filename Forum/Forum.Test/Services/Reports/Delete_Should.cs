//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Reports
{
    [TestClass]
    public class Delete_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task Delete_Should_RemoveReport_FromDatabase(long reportId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.DeleteAsync(reportId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Report was successfully deleted!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(long.MaxValue)]
        public async Task Delete_Should_NotRemoveReport_BecauseOfNonExistingId(long reportId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.DeleteAsync(reportId);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Report");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }
    }
}