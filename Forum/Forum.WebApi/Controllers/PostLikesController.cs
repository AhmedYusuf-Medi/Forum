//Local
using Forum.Models.Request.PostLike;
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
    public class PostLikesController : ControllerBase
    {
        private readonly IPostLikeService postLikeService;

        public PostLikesController(IPostLikeService postLikeService)
        {
            this.postLikeService = postLikeService;
        }

        /// <summary>
        /// Creates post like
        /// </summary>
        [HttpPost]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Create([FromBody] PostLikeRequestModel postLikeRequest)
        {
            postLikeRequest.UserId = long.Parse(this.Request.Cookies["UserId"]);

            var result = await this.postLikeService.CreateAsync(postLikeRequest);

            if (!result.IsSuccess)
            {
                return this.BadRequest(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes post like by given Id
        /// </summary>
        [HttpDelete("{id}")]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await this.postLikeService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }
    }
}
