using Forum.Models.Request.Post;
using Forum.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum.WebMVC.ViewComponents.Post
{
    public class DisplayUserPostsViewComponent : ViewComponent
    {
        private readonly IPostService postService;
        private readonly PostFilterRequestModel request;

        public DisplayUserPostsViewComponent(IPostService postService)
        {
            this.postService = postService;
            this.request = new PostFilterRequestModel()
            {
                Page = 1,
                PerPage = 5,
                MostRecently = "mr"
            };
        }

        public async Task<IViewComponentResult> InvokeAsync(long userId)
        {
            this.request.UserId = userId;

            var result = await this.postService.FilterAsync(this.request);

            return View("DisplayPosts", result.Payload.Entities);
        }
    }
}
