//Local
using Forum.Models.Request.CommentLike;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
using Forum.WebMVC.Models.Posts;
//Static
using static Forum.Service.Common.Message.Message;
//Nuget Packets
using Microsoft.AspNetCore.Mvc;
//Public
using System.Threading.Tasks;

namespace Forum.WebMVC.Controllers
{
    public class CommentLikesController : Controller
    {
        private readonly ICommentLikeService commentLikeService;
        private readonly ICommentService commentService;

        public CommentLikesController(ICommentLikeService commentLikeService, ICommentService commentService)
        {
            this.commentLikeService = commentLikeService;
            this.commentService = commentService;
        }

        [HttpPost]
        [Authorization(new string[] { Constants.User, Constants.Admin, Constants.Blocked })]
        public async Task<IActionResult> Create([FromForm]CommentLikeRequestModel commentLikeRequest, long postId, int page)
        {
            commentLikeRequest.UserId = long.Parse(this.Request.Cookies["UserId"]);

            await this.commentLikeService.CreateAsync(commentLikeRequest);

           // return this.RedirectToAction("DisplayPostComments", "Posts", new DisplayPostCommentsRequestModel { PostId = postId, Page = page});

            var likesCount = await this.commentService.GetLikesCountAsync(commentLikeRequest.CommentId);

            return Ok(new { count = likesCount });
        }
    }
}