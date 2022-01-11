//Local
using Forum.Models.Request.Comment;
using Forum.Models.Request.Post;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
using Forum.WebMVC.Models.Home;
using Forum.WebMVC.Models.Posts;
//Nuget packets
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//Public
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly ISelectListService selectListService;

        public PostsController(IPostService postService, ISelectListService selectListService)
        {
            this.postService = postService;
            this.selectListService = selectListService;
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> CreateForm()
        {
            ViewBag.Categories = new SelectList(await this.selectListService.GetAllCategoriesAsync(), "Id", "Name");
            return View();
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Create([FromForm] CreatePostRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View("CreateForm", model);
            }

            await this.postService.CreateAsync(model);

            return this.RedirectToAction("Index", "Home", new PostSortRequestModel { MostRecently = "mr"});
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> EditForm(long id, int page)
        {
            ViewBag.Categories = new SelectList(await this.selectListService.GetAllCategoriesAsync(), "Id", "Name");
            var post = await this.postService.GetByIdAsync(id);
            ViewBag.Post = post.Payload;
            ViewBag.Page = page;
            return View();
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Edit(long id, int page, [FromForm] EditPostRequestModel model)
        {
            var userId = long.Parse(this.Request.Cookies["UserId"]);

            await this.postService.EditAsync(id, userId, model);

            return this.RedirectToAction("Index", "Home", new HomeIndexRequest() { Page = page });
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Delete(long id, int page)
        {
            long? userId = null;

            if (this.Request.Cookies["Role"].Equals(Constants.User))
            {
                userId = long.Parse(this.Request.Cookies["UserId"]);
            }

            await this.postService.DeleteAsync(id, userId);

            return this.RedirectToAction("Index", "Home", new HomeIndexRequest() { Page = page });
        }

        [HttpGet]
        public async Task<IActionResult> DisplayPostComments(DisplayPostCommentsRequestModel request)
        {
            var post = await this.postService.GetByIdAsync(request.PostId);

            var model = new DisplayPostCommentsViewModel()
            {
                Post = post.Payload,
                Page = request.Page,
                Comment = new CreateCommentRequestModel()
                {
                    PostId = request.PostId
                }
            };

            ViewBag.Page = request.Page;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] PostFilterRequestModel request)
        {
            request.PerPage = 5;

            var result = await this.postService.FilterAsync(request);

            ViewBag.Page = request.Page;

            return View("Index", result.Payload);
        }
    }
}