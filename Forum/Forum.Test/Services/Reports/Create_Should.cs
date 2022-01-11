//Local
using Forum.Data;
using Forum.Models.Request.Report;
using Forum.Models.Response;
using Forum.Service;
using Forum.Service.Common.Exceptions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Reports
{
    [TestClass]
    public class Create_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2, 3, "Description")]
        [DataRow(3, 2, 2, "Description")]
        public async Task CreateShould_AddNewReport_ToDatabase(long senderId, long receiverId, long reportTypeId, string description)
        {
            var requestModel = new CreateReportRequestModel()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ReportTypeId = reportTypeId,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Report was successfully created!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0, 2, 3, "Description")]
        [DataRow(3, 0, 2, "Description")]
        [DataRow(3, 0, 0, "Description")]
        public async Task CreateShould_NotAddNewReport_ToDatabase_BecauseOfBadRequestParamters(long senderId, long receiverId, long reportTypeId, string description)
        {
            var requestModel = new CreateReportRequestModel()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ReportTypeId = reportTypeId,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                await Assert.ThrowsExceptionAsync<BadRequestException>(() => sut.CreateAsync(requestModel));
            }
        }


        [TestMethod]
        [DataRow(2, 2, 3, "Description")]
        public async Task CreateShould_NotAddNewReport_ToDatabase_BecauseCannotReportUrSelf(long senderId, long receiverId, long reportTypeId, string description)
        {
            var requestModel = new CreateReportRequestModel()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ReportTypeId = reportTypeId,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "It is not possible to report your self!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }
    }
}