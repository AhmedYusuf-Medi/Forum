using Forum.Models.Request.Category;
using Forum.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum.WebMVC.ViewComponents.Category
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        private readonly CategorySortRequestModel request;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this.request = new CategorySortRequestModel()
            {
                Page = 1,
                PerPage = 5,
                MostUploaded = "mu"
            };

            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await this.categoryService.OrderByAsync(this.request);

            return View("Card", categories.Payload.Entities);
        }
    }
}