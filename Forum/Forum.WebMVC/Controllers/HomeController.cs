//Local
using Forum.Models.Request.Post;
using Forum.Service.Common.Exceptions;
using Forum.Service.Contracts;
using Forum.WebMVC.Models.Error;
using Microsoft.AspNetCore.Diagnostics;
//Nuget packet
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
//Public
using System.Threading.Tasks;
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService postService;

        public HomeController(IPostService postService)
        {
            this.postService = postService;
        }
        
        public async Task<IActionResult> Index(PostSortRequestModel request)
        {
            request.PerPage = 5;

            ViewBag.Page = request.Page;

            var result = await this.postService.OrderByAsync(request);

            return View(result.Payload);
        }

        public IActionResult Error()
        {
            var exceptionContext = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = exceptionContext.Error.GetType().Name;

            var statusCode = (int)HttpStatusCode.InternalServerError;

            if (exception.Equals(nameof(BadRequestException)))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception.Equals(nameof(UnAuthorizationException)))
            {
                statusCode = (int)HttpStatusCode.Unauthorized;

                if (this.HttpContext.Request.Cookies.Any(c => c.Key == "Role") && this.HttpContext.Request.Cookies["Role"].Equals(Constants.Pending))
                {
                    return this.RedirectToAction("Profile", "Account");
                }
                else if (!this.HttpContext.Request.Cookies.Any(c => c.Key == "Role"))
                {
                    return this.RedirectToAction("Index", "Account");
                }
            }

            var response = new ErrorViewModel();
            response.StatusCode = statusCode;
            var err = $"Exception: {exceptionContext.Error.Message}";
            response.Message = err;

            return View("Error", response);   
        }
    }
}