//Local
using Forum.Models.Pagination;
using Forum.Models.Request.User;
using Forum.Models.Response;
using Forum.Models.Response.User;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Returns user by given Id
        /// </summary>
        [HttpGet("{id}")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Response<UserResponseModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(Response<UserResponseModel>))]
        public async Task<IActionResult> UserById(long id)
        {
            var result = await service.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }
            return this.Ok(result);
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        [HttpGet("")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<UserResponseModel>>))]
        public async Task<IActionResult> Users([FromQuery] PaginationRequestModel request)
        {
            var result = await service.GetAllAsync(request);

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes user by Id
        /// </summary>
        [HttpDelete("{id}")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await service.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }
            return this.Ok(result);
        }

        /// <summary>
        /// Block user
        /// </summary>
        [HttpPatch("block/{id}")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Block(long id)
        {
            var result = await service.BlockAsync(id);
           
            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }
            return this.Ok(result);
        }

        /// <summary>
        /// Unblock user
        /// </summary>
        [HttpPatch("unblock/{id}")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> UnBlock(long id)
        {
            var result = await service.UnBlockAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }
            return this.Ok(result);
        }

        /// <summary>
        /// Search by given parameters
        /// </summary>
        [HttpGet("searchby")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<UserResponseModel>>))]
        public async Task<IActionResult> SearchBy([FromQuery] UserSearchRequestModel model)
        {
            var result = await service.SearchByAsync(model);

            return this.Ok(result);
        }
    }
}