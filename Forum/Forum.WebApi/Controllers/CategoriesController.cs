//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Category;
using Forum.Models.Response;
using Forum.Models.Response.Category;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
using Forum.WebApi.Helpers;
//Static
using static Forum.Service.Common.Message.Message;
//Nuget packets
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Returns Category by given Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CategoryResponseModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<CategoryResponseModel>))]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await this.categoryService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Returns all categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<CategoryResponseModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestModel model)
        {
            var result = await this.categoryService.GetAllAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Creates category
        /// </summary>
        [HttpPost]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Create([FromBody] CategoryRequestModel model)
        {
            var result = await this.categoryService.CreateAsync(model);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes category by given Id
        /// </summary>
        [HttpDelete("{id}")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await this.categoryService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Edits category
        /// </summary>
        [HttpPut("{id}")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        public async Task<IActionResult> Edit(long id, [FromBody] CategoryRequestModel model)
        {
            var result = await this.categoryService.EditAsync(id, model);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Returns categories sorted by given criteria/s
        /// </summary>
        [HttpGet("sortby")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<CategoryResponseModel>>))]
        public async Task<IActionResult> OrderBy([FromQuery]CategorySortRequestModel model)
        {
            var result = await this.categoryService.OrderByAsync(model);

            return this.Ok(result);
        }
    }
}