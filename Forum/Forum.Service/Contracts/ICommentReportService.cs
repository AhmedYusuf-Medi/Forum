//Local
using Forum.Models.Pagination;
using Forum.Models.Request.CommentReport;
using Forum.Models.Response;
using Forum.Models.Response.PostReport;
using Forum.Service.Common.Extensions;
//Public
using System.Threading.Tasks;

namespace Forum.Service
{
    public interface ICommentReportService
    {
        Task<InfoResponse> CreateAsync(CreateCommentReportRequestModel model);
        Task<InfoResponse> DeleteAsync(long commentId, long reportId);
        Task<Response<Paginate<CommentReportResponseModel>>> GetAll(PaginationRequestModel request);
    }
}