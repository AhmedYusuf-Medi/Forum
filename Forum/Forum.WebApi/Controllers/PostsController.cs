//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Post;
using Forum.Models.Response;
using Forum.Models.Response.Post;
using Forum.Service.Common.Extensions;
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
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        /// <summary>
        /// Returns post by given Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<PostResponseModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<PostResponseModel>))]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await this.postService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Returns count of all existing posts
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public async Task<IActionResult> GetCount()
        {
            var result = await this.postService.GetCountAsync();

            return this.Ok(result);
        }

        /// <summary>
        /// Returns count of likes by chosen post
        /// </summary>
        [HttpGet("{id}/likes/count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public async Task<IActionResult> GetLikesCount(long id)
        {
            var result = await this.postService.GetLikesCountAsync(id);

            return this.Ok(result);
        }

        /// <summary>
        /// Returns count of comments by chosen post
        /// </summary>
        [HttpGet("{id}/comments/count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public async Task<IActionResult> GetCommentsCount(long id)
        {
            var result = await this.postService.GetCommentsCountAsync(id);

            return this.Ok(result);
        }

        /// <summary>
        /// Returns all posts
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<PostResponseModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestModel request)
        {
            var result = await this.postService.GetAllAsync(request);

            return this.Ok(result);
        }

        /// <summary>
        /// Creates post
        /// </summary>
        [HttpPost]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Create([FromForm] CreatePostRequestModel model)
        {
            if (this.Request.Cookies["Role"].Equals(Constants.User))
            {
                model.UserId = long.Parse(this.Request.Cookies["UserId"]);
            }

            var result = await this.postService.CreateAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes post by given Id
        /// </summary>
        [HttpDelete("{id}")]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Delete(long id)
        {
            InfoResponse result = new InfoResponse();

            if (this.Request.Cookies["Role"].Equals(Constants.User))
            {
                long userId = long.Parse(Request.Cookies["UserId"]);

                result = await this.postService.DeleteAsync(id, userId);
            }

            if (this.Request.Cookies["Role"].Equals(Constants.Admin))
            {
                result = await this.postService.DeleteAsync(id, null);
            }

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Edits post
        /// </summary>
        [HttpPut("{id}")]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Edit(long id, [FromForm] EditPostRequestModel model)
        {
            long userId = long.Parse(this.Request.Cookies["UserId"]);

            var result = await this.postService.EditAsync(id, userId, model);

            if (result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Returns posts filtered by given criteria/s
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<Paginate<PostResponseModel>>))]
        public async Task<IActionResult> Filter([FromQuery]PostFilterRequestModel model)
        {
            var result = await this.postService.FilterAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Returns all posts sorted by given criteria/s
        /// </summary>
        [HttpGet("sortby")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<Paginate<PostResponseModel>>))]
        public async Task<IActionResult> OrderBy([FromQuery] PostSortRequestModel model)
        {
            var result = await this.postService.OrderByAsync(model);

            return this.Ok(result);
        }
    }
}