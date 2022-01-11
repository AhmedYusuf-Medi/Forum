//Local
using Forum.Models.Request.User;
using Forum.Models.Response;
using Forum.Models.Response.User;
using Forum.Service.Common.Message;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//Public
using System;
using System.Threading.Tasks;

namespace Forum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Registers user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestModel user)
        {
            var result = await this.accountService.RegisterUserAsync(user);

            if (!result.IsSuccess)
            {
                return this.BadRequest(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Edit user profile informations
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Edit(long id, [FromForm] UserEditRequestModel user)
        {
            var result = await this.accountService.EditUserAsync(id, user);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }
            return this.Ok(result);
        }

        /// <summary>
        /// Logins user for 20 min time span and remmebers his role, userId and username
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<UserLoginResponseModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<UserLoginResponseModel>))]
        public async Task<IActionResult> LoginOperation([FromBody]UserLoginRequestModel user)
        {
            var result = await this.accountService.LoginAsync(user);

            if (!result.IsSuccess)
            {
                return this.BadRequest(result);
            }

            var currentRole = result.Payload.Role;
            var userId = result.Payload.Id;
            var username = result.Payload.Username;

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(20);

            this.Response.Cookies.Append("Role", currentRole, options);
            this.Response.Cookies.Append("UserId", userId.ToString(), options);
            this.Response.Cookies.Append("Username", username , options);

            return Ok(result.Message);
        }

        /// <summary>
        /// Logouts user
        /// </summary>
        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Message.ResponseMessages))]
        public IActionResult LogOutOperation()
        {
            this.Response.Cookies.Delete("Role");
            this.Response.Cookies.Delete("UserId");
            this.Response.Cookies.Delete("Username");
            return Ok(Message.ResponseMessages.Logout_Suceed);
        }

        /// <summary>
        /// Takes the verification url that we send to the user so he can verificates his self
        /// </summary>
        [HttpGet("verification")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Verification([FromQuery] string email, Guid code)
        {
            var result = await this.accountService.VerificationAsync(email, code);
            return Ok(result);
        }
    }
}