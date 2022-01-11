//Local
using Forum.Data;
using Forum.Models.Request.Post;
using Forum.Models.Response;
using Forum.Service;
using Forum.Service.Common.Exceptions;
using Microsoft.AspNetCore.Http;
//Nuget packets
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services.Posts
{
    [TestClass]
    public class Create_Should : BaseTest
    {
        [TestMethod]
        [DataRow(1, 1, "Title", "Description")]
        [DataRow(3, 6, "Title", "Description")]
        public async Task CreateShould_AddNewPost_ToDatabase(long userId, long categoryId, string title, string description)
        {
            var requestModel = new CreatePostRequestModel()
            {
                UserId = userId,
                CategoryId = categoryId,
                Title = title,
                Description = description,
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                var actual = await sut.CreateAsync(requestModel);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.IsSuccess);
                Assert.AreEqual(actual.Message, "Post was successfully created!");
                Assert.IsInstanceOfType(actual, typeof(InfoResponse));
            }
        }

        [TestMethod]
        [DataRow(0, 1, "Title", "Description")]
        [DataRow(1, 0, "Title", "Description")]
        [DataRow(long.MaxValue, 1, "Title", "Description")]
        [DataRow(1, long.MaxValue, "Title", "Description")]
        public async Task CreateShould_NotAddNewComment_ToDatabase(long userId, long categoryId, string title, string description)
        {
            var requestModel = new CreatePostRequestModel()
            {
                UserId = userId,
                CategoryId =categoryId,
                Title = title,
                Description = description
            };

            using (var assertContext = new ForumDbContext(this.Options))
            {
                var sut = new PostService(assertContext, this.CloudinaryService);

                await Assert.ThrowsExceptionAsync<BadRequestException>(() => sut.CreateAsync(requestModel));
            }
        }
    }
}