using Forum.Models.Request.CommentLike;
using Forum.Models.Response;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface ICommentLikeService
    {
        Task<InfoResponse> CreateAsync(CommentLikeRequestModel commentLikeRequest);
        Task<InfoResponse> DeleteAsync(long id);
    }
}