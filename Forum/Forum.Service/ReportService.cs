//Local
using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Request.Report;
using Forum.Models.Response;
using Forum.Models.Response.Report;
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
    public class ReportService : IReportService
    {
        private readonly ForumDbContext db;

        public ReportService(ForumDbContext db)
        {
            this.db = db;
        }

        public async Task<Response<ReportResponseModel>> GetByIdAsync(long id)
        {
            var report = await this.db.Reports.Where(r => r.Id == id)
                                         .Select(r => new ReportResponseModel
                                         (
                                             r.Sender.Username,
                                             r.Receiver.Username,
                                             r.ReportType.Name,
                                             r.Description
                                         ))
                                         .FirstOrDefaultAsync();

            var response = new Response<ReportResponseModel>();
            response.Payload = report;

            ValidateForNull(response, ResponseMessages.Entity_Get_Succeed, Constants.Report);

            return response;
        }

        public async Task<Response<Paginate<ReportResponseModel>>> GetAllAsync(PaginationRequestModel request)
        {
            var reports = ReportQueries.GetAllReports(this.db.Reports);

            var paginatedReports = await Paginate<ReportResponseModel>.
            ToPaginatedCollection(reports, request.Page, request.PerPage);

            var response = new Response<Paginate<ReportResponseModel>>();
            response.Payload = paginatedReports;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Reports);

            return response;
        }

        public async Task<InfoResponse> CreateAsync(CreateReportRequestModel model)
        {
            var response = new InfoResponse();

            await ForeignKeyValidations.CheckUser(model.ReceiverId, this.db,
              string.Format(ExceptionMessages.DOESNT_EXIST, Constants.User));
            await ForeignKeyValidations.CheckUser(model.SenderId, this.db,
                string.Format(ExceptionMessages.DOESNT_EXIST, Constants.User));
            await ForeignKeyValidations.CheckReportType(model.ReportTypeId, this.db,
              string.Format(ExceptionMessages.DOESNT_EXIST, Constants.Report_Type));

            if (model.SenderId.Equals(model.ReceiverId))
            {
                response.Message = ExceptionMessages.Cannot_Report_YourSelf;
                response.IsSuccess = false;
            }
            else
            {
                var report = Mapper.ToReport(model);

                await this.db.Reports.AddAsync(report);
                await this.db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Report);
            }

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id)
        {
            var report = await this.db.Reports.FirstOrDefaultAsync(c => c.Id == id);

            var response = new InfoResponse();

            ValidateForNull(report, response, ResponseMessages.Entity_Delete_Succeed, Constants.Report);

            if (response.IsSuccess)
            {
                this.db.Reports.Remove(report);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response<Paginate<ReportResponseModel>>> FilterAsync(ReportFilterRequestModel model)
        {
            var reports = this.db.Reports.AsQueryable();

            if (CriteriaValidations.BySingleCriteria(model.Sender))
            {
                reports = reports.Where(r => r.Sender.Username == model.Sender);
            }

            if (CriteriaValidations.BySingleCriteria(model.Receiver))
            {
                reports = reports.Where(r => r.Receiver.Username == model.Receiver);
            }

            if (CriteriaValidations.BySingleNullableId(model.UserId))
            {
                reports = reports.Where(r => r.ReceiverId == model.UserId || r.SenderId == model.UserId);
            }

            var reportResponse = ReportQueries.GetAllReports(reports);

            var paginatedReports = await Paginate<ReportResponseModel>
                .ToPaginatedCollection(reportResponse, model.Page, model.PerPage);

            var response = new Response<Paginate<ReportResponseModel>>();
            response.Payload = paginatedReports;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Reports);

            return response;
        }

        public async Task<Response<Paginate<ReportResponseModel>>> OrderByAsync(ReportSortRequestModel model)
        {
            var reports = this.db.Reports.AsQueryable();

            if (CriteriaValidations.BySingleCriteria(model.MostRecently))
            {
                reports = reports.OrderByDescending(r => r.CreatedOn);
            }

            var reportResponse = ReportQueries.GetAllReports(reports);

            var paginatedReports = await Paginate<ReportResponseModel>
                .ToPaginatedCollection(reportResponse, model.Page, model.PerPage);

            var response = new Response<Paginate<ReportResponseModel>>();
            response.Payload = paginatedReports;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Reports);

            return response;
        }
    }
}