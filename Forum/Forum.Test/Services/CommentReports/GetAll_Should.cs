//Local
using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Response.PostReport;
using Forum.Service;
using Forum.Service.Common.Extensions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.PostReports
{
    [TestClass]
    public class GetAll_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 1)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_With_ReportModels(int page, int perPage)
        {
            var requestModel = new PaginationRequestModel()
            {
                Page = page,
                PerPage = perPage
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var mockReportService = new Mock<ReportService>(assertContext);

                var sut = new PostReportService(assertContext, mockReportService.Object);

                var actual = await sut.GetAll(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all post reports!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<PostReportResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(PostReportResponseModel));
            }
        }
    }
}