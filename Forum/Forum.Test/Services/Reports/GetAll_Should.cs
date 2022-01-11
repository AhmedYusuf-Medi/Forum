//Local
using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Response.Report;
using Forum.Service;
using Forum.Service.Common.Extensions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Reports
{
    [TestClass]
    public class GetAll_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_With_ReportModels(int page, int perPage)
        {
            var requestModel = new PaginationRequestModel()
            {
                Page = page,
                PerPage = perPage
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.GetAllAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all reports!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<ReportResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(ReportResponseModel));
            }
        }
    }
}