//Local
using Forum.Models.Request.User;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
//Nuget packets
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//Public
using System;
using System.Linq;
using System.Threading.Tasks;
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IUserService userService;
        private readonly IMailService mailService;
        private readonly CookieOptions options;

        public AccountController(IAccountService service, IUserService userService, IMailService mailService)
        {
            this.accountService = service;
            this.userService = userService;
            this.mailService = mailService;
            this.options = new CookieOptions();
        }

        public IActionResult Index()
        {
            return View("index");
        }

        [HttpPost]
        public async Task<IActionResult> LoginOperation([FromForm] UserLoginRequestModel user)
        {
            if (!this.ModelState.IsValid)
            {
                return View("index");
            }

            var result = await this.accountService.LoginAsync(user);

            if (!result.IsSuccess)
            {
                ViewBag.LoginError = "Invalid email or password!";
                return View("index");
            }

            var currentRole = result.Payload.Role;
            var userId = result.Payload.Id;
            var username = result.Payload.Username;
            var avatar = result.Payload.Avatar;


            options.Expires = DateTime.Now.AddMinutes(20);

            this.Response.Cookies.Append("Role", currentRole, options);
            this.Response.Cookies.Append("UserId", userId.ToString(), options);
            this.Response.Cookies.Append("Username", username, options);
            this.Response.Cookies.Append("Avatar", avatar, options);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            this.Response.Cookies.Delete("Role");
            this.Response.Cookies.Delete("UserId");
            this.Response.Cookies.Delete("Username");
            this.Response.Cookies.Delete("Avatar");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegisterForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserRegisterRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View("RegisterForm", model);
            }
            
            var result = await this.accountService.RegisterUserAsync(model);

            if (!result.IsSuccess)
            {
                ViewBag.UserExist = result.Message;
                return View("RegisterForm");
            }

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Verification([FromQuery] string email, Guid code)
        {
            var result = await this.accountService.VerificationAsync(email, code);

            if (result.IsSuccess)
            {
                if (Request.Cookies.Any(c => c.Key == "Role"))
                {
                    options.Expires = DateTime.Now.AddMinutes(20);
                    this.Response.Cookies.Append("Role", "User", options);
                    
                    return RedirectToAction("Profile", "Account");
                }
               
                return View("Index");
            }
            return Ok(result.Message); // to refactor
        }

        [Authorization(new string[] { Constants.Admin, Constants.User})]
        public async Task<IActionResult> EditProfile()
        {
            var currUserId = long.Parse(this.HttpContext.Request.Cookies["UserId"]);
            var response = await userService.GetByIdAsync(currUserId);
            ViewBag.CurrentUser = response.Payload;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] UserEditRequestModel user)
        {
            if (!this.ModelState.IsValid)
            {
                var Id = long.Parse(this.HttpContext.Request.Cookies["UserId"]);
                var response = await userService.GetByIdAsync(Id);
                ViewBag.CurrentUser = response.Payload;

                return View("EditProfile", user);
            }

            var currUserId = long.Parse(this.HttpContext.Request.Cookies["UserId"]);
            var result = await this.accountService.EditUserAsync(currUserId, user);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            var updatedUser = await userService.GetByIdAsync(currUserId);
            options.Expires = DateTime.Now.AddMinutes(20);
            this.Response.Cookies.Append("Avatar", updatedUser.Payload.PicturePath, options);

            TempData["Success"] = "Successfully updated your profile!";
            return RedirectToAction("Profile", "Account");
        }

        [Authorization(new string[] { Constants.Admin, Constants.User, Constants.Blocked, "Pending" })]
        public async Task<IActionResult> Profile()
        {
            var currUserId = long.Parse(this.HttpContext.Request.Cookies["UserId"]);
            var response = await userService.GetByIdAsync(currUserId);
            ViewBag.CurrentUser = response.Payload;
            return View();
        }

        public async Task<IActionResult> ResendEmail(string email, Guid code) 
        {
            var sendEmail = await mailService.SendMailAsync(email, code);
            return RedirectToAction("Profile");
        }
    }
}