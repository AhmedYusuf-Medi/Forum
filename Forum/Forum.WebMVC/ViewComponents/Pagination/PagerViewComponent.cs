using Forum.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum.WebMVC.ViewComponents.Pagination
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(Metadata result)
        {
            return Task.FromResult((IViewComponentResult)View("Pager", result));
        }
    }
}