using Forum.Models.Pagination;
using Forum.Models.Request.Comment;
using Forum.Models.Response;
using Forum.Models.Response.Comment;
using Forum.Service.Common.Extensions;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface ICommentService
    {
        Task<InfoResponse> CreateAsync(CreateCommentRequestModel model);
        Task<InfoResponse> DeleteAsync(long id, long? userId);
        Task<InfoResponse> EditAsync(long id, long userId, EditCommentRequestModel model);
        Task<Response<Paginate<CommentResponseModel>>> GetAllAsync(PaginationRequestModel request);
        Task<Response<CommentResponseModel>> GetByIdAsync(long id);
        Task<long> GetPostCommentsCountAsync(long postId);
        Task<long> GetCountAsync();
        Task<Response<Paginate<CommentResponseModel>>> OrderByAsync(CommentSortRequestModel model);
        Task<Response<Paginate<CommentResponseModel>>> FilterAsync(CommentFilterRequestModel model);
        Task<long> GetLikesCountAsync(long commentId);
    }
}