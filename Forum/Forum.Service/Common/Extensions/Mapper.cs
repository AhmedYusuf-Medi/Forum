//Local
using Forum.Models.Entities;
using Forum.Models.Request.Category;
using Forum.Models.Request.Comment;
using Forum.Models.Request.Post;
using Forum.Models.Request.Report;
using Forum.Models.Request.User;

namespace Forum.Service.Common.Extensions
{
    public static class Mapper
    {
        public static User ToUser(UserRegisterRequestModel user)
        {
            return new User()
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Password = user.Password,
            };
        }

        public static Report ToReport(CreateReportRequestModel report)
        {
            return new Report()
            {
                ReceiverId =  report.ReceiverId,
                SenderId =  report.SenderId,
                ReportTypeId =  report.ReportTypeId,
                Description = report.Description
            };
        }

        public static Post ToPost(CreatePostRequestModel model)
        {
            return new Post
            {
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
                CategoryId = model.CategoryId
            };
        }

        public static  Category ToCategory(CategoryRequestModel model)
        {
            return new Category
            {
                Name = model.Name
            };
        }

        public static Comment ToComment(CreateCommentRequestModel model)
        {
            return new Comment
            {
                UserId = model.UserId,
                PostId = model.PostId,
                Description = model.Description,
            };
        }
    }
}
