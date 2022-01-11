using Forum.Models.Pagination;
using Forum.Models.Request.User;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;

        public AdminController(IUserService userService, ICategoryService categoryService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
        }

        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Index(string username, string email, string displayName, int page = 1)
        {
            var parameters = new UserSearchRequestModel()
            {
                PerPage = 3,
                Page = page,
                Username = username,
                Email = email,
                DisplayName = displayName
            };

            var result = await this.userService.SearchByAsync(parameters);

            var categoryRequest = new PaginationRequestModel()
            {
                PerPage = 5,
                Page = page
            };

            return View(result.Payload);
        }

        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Block([FromForm] long id)
        {
            var result = await userService.BlockAsync(id);

            return RedirectToAction("Index", "Admin");
        }

        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> UnBlock([FromForm] long id)
        {
            var result = await userService.UnBlockAsync(id);

            return RedirectToAction("Index", "Admin");
        }

        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Categories(int page = 1)
        {

            var categoryRequest = new PaginationRequestModel()
            {
                PerPage = 5,
                Page = page
            };

            var result = await this.categoryService.GetAllAsync(categoryRequest);

            return View(result.Payload);
        }
    }
}
