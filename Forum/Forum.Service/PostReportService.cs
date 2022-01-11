//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Pagination;
using Forum.Models.Request.PostReport;
using Forum.Models.Request.Report;
using Forum.Models.Response;
using Forum.Models.Response.PostReport;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Linq;
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Extensions.Validator;
using static Forum.Service.Common.Message.Message;

namespace Forum.Service
{
    public class PostReportService : IPostReportService
    {
        private readonly ForumDbContext db;
        private readonly IReportService reportService;

        public PostReportService(ForumDbContext db, IReportService reportService)
        {
            this.db = db;
            this.reportService = reportService;
        }

        public async Task<InfoResponse> CreateAsync(CreatePostReportRequestModel model)
        {
            await ForeignKeyValidations.CheckPost(model.PostId, this.db,
             string.Format(ExceptionMessages.DOESNT_EXIST, Constants.Post));

            var response = new InfoResponse();

            var isFirstReport = await this.db.PostReports
                .AnyAsync(pr => pr.PostId == model.PostId && pr.Report.ReceiverId == model.ReceiverId && pr.Report.SenderId == model.SenderId);

            ForeignKeyValidations.CheckReport(isFirstReport, Constants.Post.ToLower(), response);

            if (response.IsSuccess)
            {
                await this.reportService.CreateAsync(new CreateReportRequestModel()
                {
                    ReceiverId = model.ReceiverId,
                    SenderId = model.SenderId,
                    Description = model.Description,
                    ReportTypeId = model.ReportTypeId
                });

                var report = await this.db.Reports.Select(r => new Report() { Id = r.Id })
                                                  .OrderByDescending(r => r.Id)
                                                  .FirstOrDefaultAsync();

                var postReport = new PostReport()
                {
                    ReportId = report.Id,
                    PostId = model.PostId
                };

                await this.db.PostReports.AddAsync(postReport);
                await this.db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Post_Report);
            }

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long postId, long reportId)
        {
            var postReport = await this.db.PostReports.FirstOrDefaultAsync(r => r.PostId == postId && r.ReportId == reportId);

            var response = new InfoResponse();

            ValidateForNull(postReport, response, ResponseMessages.Entity_Delete_Succeed, Constants.Post_Report);

            if (response.IsSuccess)
            {
                this.db.PostReports.Remove(postReport);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response<Paginate<PostReportResponseModel>>> GetAll(PaginationRequestModel request)
        {
            var postReports = this.db.PostReports.Select(r => new PostReportResponseModel
            (
                r.Report.Receiver.Username,
                r.Report.Sender.Username,
                r.PostId,
                r.Report.ReportType.Name
            ));

            var paginatedReports = await Paginate<PostReportResponseModel>.
             ToPaginatedCollection(postReports, request.Page, request.PerPage);

            var response = new Response<Paginate<PostReportResponseModel>>();
            response.Payload = paginatedReports;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Post_Reports);

            return response;
        }
    }
}