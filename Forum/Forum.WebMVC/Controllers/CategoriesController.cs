using Forum.Models.Request.Category;
using Forum.Service.Contracts;
using Forum.WebMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Forum.Service.Common.Message.Message;

namespace Forum.WebMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Create([Bind] string name)
        {
            var model = new CategoryRequestModel();
            model.Name = name;
            
            var result = await this.categoryService.CreateAsync(model);

            return RedirectToAction("Index", "Admin");
        }
    }
}