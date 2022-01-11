//Local
using Forum.Models.Request.Comment;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
using Forum.WebMVC.Models.Posts;
//Nuget packets
using Microsoft.AspNetCore.Mvc;
//Public
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateCommentRequestModel request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("DisplayPostComments", "Posts", new DisplayPostCommentsRequestModel() { PostId = request.PostId });
            }

            await this.commentService.CreateAsync(request);

            return this.RedirectToAction("DisplayPostComments", "Posts" , new DisplayPostCommentsRequestModel() { PostId = request.PostId});
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> EditForm(long id)
        {
            var comment = await this.commentService.GetByIdAsync(id);
            ViewBag.Comment = comment.Payload;
            return View();
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Edit(long id,[FromForm]EditCommentRequestModel model)
        {
            var userId = long.Parse(this.Request.Cookies["UserId"]);

            await this.commentService.EditAsync(id, userId, model);

            return this.RedirectToAction("Index", "Home");
        }

        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Delete (long id, long postId)
        {
            long? userId = null;

            if (this.Request.Cookies["Role"].Equals(Constants.User))
            {
                userId = long.Parse(this.Request.Cookies["UserId"]);
            }

            await this.commentService.DeleteAsync(id, userId);

            return this.RedirectToAction("DisplayPostComments", "Posts", new DisplayPostCommentsRequestModel() { PostId = postId });
        }
    }
}