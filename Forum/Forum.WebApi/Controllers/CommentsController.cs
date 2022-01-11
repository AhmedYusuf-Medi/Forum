//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Comment;
using Forum.Models.Response;
using Forum.Models.Response.Comment;
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
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        /// <summary>
        /// Returns comment by given Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CommentResponseModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<CommentResponseModel>))]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await this.commentService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Returns all comments
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<CommentResponseModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestModel model)
        {
            var result = await this.commentService.GetAllAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Creates comment
        /// </summary>
        [HttpPost]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Create([FromForm] CreateCommentRequestModel model)
        {
            if (this.Request.Cookies["Role"].Equals(Constants.User))
            {
                model.UserId = long.Parse(Request.Cookies["UserId"]);
            }

            var result = await this.commentService.CreateAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes comment by given Id
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

                result = await this.commentService.DeleteAsync(id, userId);
            }

            if (this.Request.Cookies["Role"].Equals(Constants.Admin))
            {
                result = await this.commentService.DeleteAsync(id, null);
            }

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Edits comment
        /// </summary>
        [HttpPut("{id}")]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Edit(long id, [FromForm] EditCommentRequestModel model)
        {
            long userId = long.Parse(Request.Cookies["UserId"]);

            var result = await this.commentService.EditAsync(id, userId, model);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Returns comments sorted by given criteria/s
        /// </summary>
        [HttpGet("sortby")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<CommentResponseModel>>))]
        public async Task<IActionResult> Orderby([FromQuery] CommentSortRequestModel model)
        {
            var result = await this.commentService.OrderByAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Returns comments filtered by given criteria/s
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<CommentResponseModel>>))]
        public async Task<IActionResult> Filter([FromQuery] CommentFilterRequestModel model)
        {
            if (Request.Cookies["Role"] == "User")
            {
                model.UserId = long.Parse(Request.Cookies["UserId"]);
            }

            var result = await this.commentService.FilterAsync(model);

            return this.Ok(result);
        }
    }
}