//Local
using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Request.Category;
using Forum.Models.Response;
using Forum.Models.Response.Category;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Linq;
using System.Threading.Tasks;
using static Forum.Service.Common.Extensions.Validator;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ForumDbContext db;

        public CategoryService(ForumDbContext db)
        {
            this.db = db;
        }

        public async Task<Response<CategoryResponseModel>> GetByIdAsync(long id)
        {
            var category = await this.db.Categories.Where(c => c.Id == id)
                .Select(c => new CategoryResponseModel
                  (
                      c.Name,
                      c.Posts.Count,
                      c.Id
                  ))
                .FirstOrDefaultAsync();

            var response = new Response<CategoryResponseModel>();
            response.Payload = category;

            ValidateForNull(response, ResponseMessages.Entity_Get_Succeed, Constants.Category);

            return response;
        }

        public async Task<Response<Paginate<CategoryResponseModel>>> GetAllAsync(PaginationRequestModel request)
        {
            var categories = this.db.Categories
                .Select(c => new CategoryResponseModel
                   (
                       c.Name,
                       c.Posts.Count,
                       c.Id
                   ));

            var paginatedCategories = await Paginate<CategoryResponseModel>
                .ToPaginatedCollection(categories, request.Page, request.PerPage);

            var response = new Response<Paginate<CategoryResponseModel>>();
            response.Payload = paginatedCategories;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Categories);
            response.IsSuccess = true;

            return response;
        }

        public async Task<InfoResponse> CreateAsync(CategoryRequestModel model)
        {
            var category = Mapper.ToCategory(model);

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();

            var response = new InfoResponse();
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Category);

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(p => p.Id == id);

            var response = new InfoResponse();

            ValidateForNull(category, response, ResponseMessages.Entity_Delete_Succeed, Constants.Category);

            if (response.IsSuccess)
            {
                this.db.Categories.Remove(category);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<InfoResponse> EditAsync(long id, CategoryRequestModel model)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            var response = new InfoResponse();

            ValidateForNull(category, response, ResponseMessages.Entity_Edit_Succeed, Constants.Category);

            if (response.IsSuccess)
            {
                if (DataValidations.IsStringPropertyValid(model.Name, category.Name))
                {
                    category.Name = model.Name;
                    await this.db.SaveChangesAsync();
                }
            }

            return response;
        }

        public async Task<Response<Paginate<CategoryResponseModel>>> OrderByAsync(CategorySortRequestModel model)
        {
            var categories = this.db.Categories.AsQueryable()
                .OrderByDescending(c => 1);

            if (CriteriaValidations.BySingleCriteria(model.MostUploaded))
            {
                categories = categories.ThenByDescending(c => c.Posts.Count());
            }

            if (CriteriaValidations.BySingleCriteria(model.MostRecently))
            {
                categories = categories.OrderByDescending(c => c.CreatedOn);
            }

            var responseCategories = categories.Select(c => new CategoryResponseModel
                                     (
                                         c.Name,
                                         c.Posts.Count,
                                         c.Id
                                     ));

            var payload = await Paginate<CategoryResponseModel>.ToPaginatedCollection(responseCategories, model.Page, model.PerPage);

            var response = new Response<Paginate<CategoryResponseModel>>();
            response.Payload = payload;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Categories);

            return response;
        }
    }
}