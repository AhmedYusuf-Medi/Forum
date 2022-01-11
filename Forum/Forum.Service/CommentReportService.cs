//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Pagination;
using Forum.Models.Request.CommentReport;
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
    public class CommentReportService : ICommentReportService
    {
        private readonly ForumDbContext db;
        private readonly IReportService reportService;

        public CommentReportService(ForumDbContext db, IReportService reportService)
        {
            this.db = db;
            this.reportService = reportService;
        }

        public async Task<InfoResponse> CreateAsync(CreateCommentReportRequestModel model)
        {
            await ForeignKeyValidations.CheckComment(model.CommentId, this.db,
             string.Format(ExceptionMessages.DOESNT_EXIST, Constants.Comment));

            var response = new InfoResponse();

            var isFirstReport = await this.db.CommentReports
                .AnyAsync(pr => pr.CommentId == model.CommentId && pr.Report.ReceiverId == model.ReceiverId && pr.Report.SenderId == model.SenderId);

            ForeignKeyValidations.CheckReport(isFirstReport, Constants.Comment.ToLower(), response);

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

                var commentReport = new CommentReport()
                {
                    ReportId = report.Id,
                    CommentId = model.CommentId
                };

                await this.db.CommentReports.AddAsync(commentReport);
                await this.db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Comment_Report);
            }

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long commentId, long reportId)
        {
            var commentReport = await this.db.CommentReports.FirstOrDefaultAsync(r => r.CommentId == commentId && r.ReportId == reportId);

            var response = new InfoResponse();

            ValidateForNull(commentReport, response, ResponseMessages.Entity_Delete_Succeed, Constants.Comment_Report);

            if (response.IsSuccess)
            {
                this.db.CommentReports.Remove(commentReport);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response<Paginate<CommentReportResponseModel>>> GetAll(PaginationRequestModel request)
        {
            var postReports = this.db.CommentReports.Select(r => new CommentReportResponseModel
            (
                r.Report.Receiver.Username,
                r.Report.Sender.Username,
                r.CommentId,
                r.Report.ReportType.Name
            ));

            var paginatedReports = await Paginate<CommentReportResponseModel>.
             ToPaginatedCollection(postReports, request.Page, request.PerPage);

            var response = new Response<Paginate<CommentReportResponseModel>>();
            response.Payload = paginatedReports;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Comment_Reports);

            return response;
        }
    }
}