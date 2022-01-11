//Local
using Forum.Models.Request.CommentLike;
using Forum.Models.Response;
using Forum.Service.Contracts;
using Forum.WebApi.Helpers;
//Static
using static Forum.Service.Common.Message.Message;
//Nuget packets
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//Public
using System.Threading.Tasks;

namespace Forum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLikesController : ControllerBase
    {
        private readonly ICommentLikeService commentLikeService;

        public CommentLikesController(ICommentLikeService commentLikeService)
        {
            this.commentLikeService = commentLikeService;
        }

        /// <summary>
        /// Creates comment like
        /// </summary>
        [HttpPost]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Create([FromBody] CommentLikeRequestModel commentLikeRequest)
        {
            commentLikeRequest.UserId = long.Parse(this.Request.Cookies["UserId"]);

            var result = await this.commentLikeService.CreateAsync(commentLikeRequest);

            if (!result.IsSuccess)
            {
                return this.BadRequest(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes comment like
        /// </summary>
        [HttpDelete("{id}")]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await this.commentLikeService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }
    }
}