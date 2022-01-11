using Forum.Models.Request.PostLike;
using Forum.Models.Response;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IPostLikeService
    {
        Task<InfoResponse> CreateAsync(PostLikeRequestModel postLikeRequest);
        Task<InfoResponse> DeleteAsync(long id);
    }
}