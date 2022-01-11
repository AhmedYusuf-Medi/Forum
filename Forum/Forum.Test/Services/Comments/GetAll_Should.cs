//Local
using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Response.Comment;
using Forum.Service;
using Forum.Service.Common.Extensions;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
//Public
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Test.Services.Comments
{
    [TestClass]
    public class GetAll_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 3)]
        [DataRow(2, 5)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_With_CommentResponseModels(int page, int perPage)
        {
            var requestModel = new PaginationRequestModel()
            {
                Page = page,
                PerPage = perPage
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetAllAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Payload.Metadata);
                Assert.IsNotNull(actual.Payload.Entities);
                Assert.AreEqual(actual.Payload.Entities.Count(), perPage);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Successfully got all comments!");
                Assert.IsInstanceOfType(actual.Payload, typeof(Paginate<CommentResponseModel>));
                CollectionAssert.AllItemsAreInstancesOfType(actual.Payload.Entities.ToList(), typeof(CommentResponseModel));
            }
        }

        [TestMethod]
        [DataRow(0, 3)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_FromPageOne_WhenGivenPageIsNegative(int page, int perPage)
        {
            var requestModel = new PaginationRequestModel()
            {
                Page = page,
                PerPage = perPage
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetAllAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.AreEqual(actual.Payload.Metadata.CurrentPage, 1);
            }
        }

        [TestMethod]
        [DataRow(1, int.MaxValue)]
        public async Task GetAll_ShouldReturn_PaginatedCollection_FromPerPageMaxCount_WhenGivenPerPage_IsBiggerThanTheCount(int page, int perPage)
        {
            var requestModel = new PaginationRequestModel()
            {
                Page = page,
                PerPage = perPage
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new CommentService(assertContext, this.CloudinaryService);

                var actual = await sut.GetAllAsync(requestModel);

                long actualCount = 13;

                Assert.IsNotNull(actual);
                Assert.AreEqual(actual.Payload.Entities.Count(), actualCount);
            }
        }
    }
}