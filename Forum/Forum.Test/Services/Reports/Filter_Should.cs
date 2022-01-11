//Local
using Forum.Data;
using Forum.Models.Request.Report;
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
    public class Filter_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 2, "stevenvselenski", null, null)]
        [DataRow(1, 2, null, "muthkabarona", null)]
        [DataRow(1, 2, null, null, (long)1)]
        public async Task Filter_ShouldReturnSorted_PaginatedCollection_With_PostResponseModels(int page, int perPage,
            string sender, string receiver, long? userId)
        {
            var requestModel = new ReportFilterRequestModel()
            {
                Page = page,
                PerPage = perPage,
                Sender = sender,
                Receiver = receiver,
                UserId = userId
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new ReportService(assertContext);

                var actual = await sut.FilterAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all reports!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<ReportResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(ReportResponseModel));
            }
        }
    }
}