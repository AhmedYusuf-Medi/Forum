//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Service;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.CommentReports
{
    [TestClass]
    public class Delete_Should : BaseTest
    {
        [TestMethod]
        [DataRow(3, 3)]
        public async Task Delete_Should_RemovePostReport_FromDatabase(long reportId, long commentId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var mockReportService = new Mock<ReportService>(assertContext);

                var sut = new CommentReportService(assertContext, mockReportService.Object);

                var actual = await sut.DeleteAsync(commentId, reportId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Comment Report was successfully deleted!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(0, 1)]
        [DataRow(1, long.MaxValue)]
        [DataRow(long.MaxValue, 1)]
        public async Task Delete_Should_NotRemovePostReport_BecauseOfNonExstingId(long reportId, long commentId)
        {
            using (var assertContext = new ForumDbContext(this.Options))
            {
                var mockReportService = new Mock<ReportService>(assertContext);

                var sut = new CommentReportService(assertContext, mockReportService.Object);

                var actual = await sut.DeleteAsync(commentId, reportId);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Doesn't exist such a Comment Report");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }
    }
}