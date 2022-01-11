using Forum.Models.Pagination;
using Forum.Models.Request.PostReport;
using Forum.Models.Response;
using Forum.Models.Response.PostReport;
using Forum.Service.Common.Extensions;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IPostReportService
    {
        Task<InfoResponse> CreateAsync(CreatePostReportRequestModel model);
        Task<InfoResponse> DeleteAsync(long postId, long reportId);
        Task<Response<Paginate<PostReportResponseModel>>> GetAll(PaginationRequestModel request);
    }
}