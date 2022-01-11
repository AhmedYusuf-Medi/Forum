//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Category;
using Forum.Models.Response;
using Forum.Models.Response.Category;
using Forum.Service.Common.Extensions;
//Public
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface ICategoryService
    {
        Task<Response<CategoryResponseModel>> GetByIdAsync(long id);
        Task<Response<Paginate<CategoryResponseModel>>> GetAllAsync(PaginationRequestModel request);
        Task<InfoResponse> CreateAsync(CategoryRequestModel model);
        Task<InfoResponse> DeleteAsync(long id);
        Task<InfoResponse> EditAsync(long id, CategoryRequestModel model);
        Task<Response<Paginate<CategoryResponseModel>>> OrderByAsync(CategorySortRequestModel model);
    }
}