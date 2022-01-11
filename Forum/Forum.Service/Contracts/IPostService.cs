//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Post;
using Forum.Models.Response;
using Forum.Models.Response.Post;
using Forum.Service.Common.Extensions;
//Public
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IPostService
    {
        Task<long> GetCountAsync();
        Task<long> GetLikesCountAsync(long postId);
        Task<long> GetCommentsCountAsync(long postId);
        Task<Response<PostResponseModel>> GetByIdAsync(long id);
        Task<Response<Paginate<PostResponseModel>>> GetAllAsync(PaginationRequestModel request);
        Task<InfoResponse> CreateAsync(CreatePostRequestModel model);
        Task<InfoResponse> DeleteAsync(long id, long? userId);
        Task<InfoResponse> EditAsync(long id, long userId, EditPostRequestModel model);
        Task<Response<Paginate<PostResponseModel>>> FilterAsync(PostFilterRequestModel model);
        Task<Response<Paginate<PostResponseModel>>> OrderByAsync(PostSortRequestModel model);
    }
}