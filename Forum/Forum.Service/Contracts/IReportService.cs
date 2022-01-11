//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Report;
using Forum.Models.Response;
using Forum.Models.Response.Report;
using Forum.Service.Common.Extensions;
//Public
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IReportService
    {
        Task<InfoResponse> CreateAsync(CreateReportRequestModel model);
        Task<InfoResponse> DeleteAsync(long id);
        Task<Response<Paginate<ReportResponseModel>>> FilterAsync(ReportFilterRequestModel model);
        Task<Response<Paginate<ReportResponseModel>>> OrderByAsync(ReportSortRequestModel model);
        Task<Response<Paginate<ReportResponseModel>>> GetAllAsync(PaginationRequestModel request);
        Task<Response<ReportResponseModel>> GetByIdAsync(long id);
    }
}