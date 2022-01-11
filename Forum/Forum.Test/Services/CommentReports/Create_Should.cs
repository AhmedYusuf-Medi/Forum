//Local
using Forum.Data;
using Forum.Models.Request.CommentReport;
using Forum.Models.Response;
using Forum.Service;
using Forum.Service.Common.Exceptions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.CommentReports
{
    [TestClass]
    public class Create_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2, 4, 2, "Description")]
        [DataRow(2, 3, 5, 3, "Description")]
        public async Task CreateShould_AddNewCommentReport_ToDatabase(long senderId, long receiverId, long commentId, long reportTypeId, string description)
        {
            var requestModel = new CreateCommentReportRequestModel()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ReportTypeId = reportTypeId,
                Description = description,
                CommentId = commentId
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var mockReportService = new Mock<ReportService>(assertContext);

                var sut = new CommentReportService(assertContext, mockReportService.Object);

                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Comment Report was successfully created!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 2, 4, 2, "Description")]
        [DataRow(2, 3, 5, 3, "Description")]
        public async Task CreateShould_NotAddNewCommentReport_BecauseOfAlreadyExisting(long senderId, long receiverId, long commentId, long reportTypeId, string description)
        {
            var requestModel = new CreateCommentReportRequestModel()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ReportTypeId = reportTypeId,
                Description = description,
                CommentId = commentId
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var mockReportService = new Mock<ReportService>(assertContext);

                var sut = new CommentReportService(assertContext, mockReportService.Object);

                await sut.CreateAsync(requestModel);
                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Cannot report comment more than once!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(1, 2, 0, 2, "Description")]
        [DataRow(2, 3, long.MaxValue, 3, "Description")]
        public async Task CreateShould_NotAddNewCommentReport_BecauseOfNonExistingPost(long senderId, long receiverId, long commentId, long reportTypeId, string description)
        {
            var requestModel = new CreateCommentReportRequestModel()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ReportTypeId = reportTypeId,
                Description = description,
                CommentId = commentId
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var mockPostService = new Mock<ReportService>(assertContext);

                var sut = new CommentReportService(assertContext, mockPostService.Object);

                await Assert.ThrowsExceptionAsync<BadRequestException>(() => sut.CreateAsync(requestModel));
            }
        }
    }
}