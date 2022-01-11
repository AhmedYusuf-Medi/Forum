//Local
using Forum.Models.Request.PostLike;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
using Forum.WebMVC.Models.Home;
//Nuget packets
using Microsoft.AspNetCore.Mvc;
//Public
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class PostLikesController : Controller
    {
        private readonly IPostLikeService postLikeService;
        private readonly IPostService postService;

        public PostLikesController(IPostLikeService postLikeService,
              IPostService postService)
        {
            this.postLikeService = postLikeService;
            this.postService = postService;
        }

        [HttpPost]
        [Authorization(new string[] { Constants.User, Constants.Admin, Constants.Blocked })]
        public async Task<IActionResult> Create([FromForm] PostLikeRequestModel postLikeRequest)
        {
            postLikeRequest.UserId = long.Parse(this.Request.Cookies["UserId"]);

            await this.postLikeService.CreateAsync(postLikeRequest);

            var likesCount = await this.postService.GetLikesCountAsync(postLikeRequest.PostId);

            return Ok(new { count = likesCount });
        }
    }
}