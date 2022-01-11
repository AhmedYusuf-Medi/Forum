//Local
using Forum.Models.Pagination;
using Forum.Models.Request.User;
using Forum.Models.Response;
using Forum.Models.Response.User;
using Forum.Service.Common.Extensions;
//Public
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IUserService
    {
        Task<InfoResponse> DeleteAsync(long id);

        Task<Response<UserResponseModel>> GetByIdAsync(long id);

        Task<long> GetCountAsync();

        Task<Response<Paginate<UserResponseModel>>> GetAllAsync(PaginationRequestModel request);

        Task<InfoResponse> BlockAsync(long id);

        Task<InfoResponse> UnBlockAsync(long id);

        Task<Response<Paginate<UserResponseModel>>> SearchByAsync(UserSearchRequestModel model);
    }
}