using Forum.Models.Request.Comment;
using Forum.Service.Contracts;
using Forum.WebMVC.Models.Posts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum.WebMVC.ViewComponents.Comment
{
    public class DisplayPostCommentsViewComponent : ViewComponent
    {
        private readonly ICommentService commentService;
        private readonly CommentFilterRequestModel request;

        public DisplayPostCommentsViewComponent(ICommentService commentService)
        {
            this.commentService = commentService;
            this.request = new CommentFilterRequestModel()
            {
                Page = 1,
                PerPage = 2,
                MostRecently = "mr"
            };
        }

        public async Task<IViewComponentResult> InvokeAsync(DisplayPostCommentsViewModel model)
        {
            this.request.Page = model.Page;
            this.request.PostId = model.Post.Id;

            var result = await this.commentService.FilterAsync(this.request);

            return View("DisplayComments", result.Payload);
        }
    }
}
