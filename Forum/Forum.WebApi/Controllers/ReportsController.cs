//Local
using Forum.Models.Pagination;
using Forum.Models.Request.Report;
using Forum.Models.Response;
using Forum.Models.Response.Report;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
using Forum.WebApi.Helpers;
//Nuget packets
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//Public
using System.Threading.Tasks;
using static Forum.Service.Common.Message.Message;

namespace Forum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        /// <summary>
        /// Returns all reports
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<ReportResponseModel>>))]
        public async Task<IActionResult> GetAll([FromQuery]PaginationRequestModel request)
        {
            var result = await this.reportService.GetAllAsync(request);

            return this.Ok(result);
        }

        /// <summary>
        /// Returns report by given Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<ReportResponseModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<ReportResponseModel>))]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await this.reportService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Creates report
        /// </summary>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Create([FromBody] CreateReportRequestModel model)
        {
            var result = await this.reportService.CreateAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes report
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await this.reportService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return this.NotFound(result);
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Filters reports by given criterias
        /// </summary>
        [HttpGet("filter")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<ReportResponseModel>>))]
        public async Task<IActionResult> Filter([FromQuery]ReportFilterRequestModel model)
        {
            var result = await this.reportService.FilterAsync(model);

            return this.Ok(result);
        }

        /// <summary>
        /// Returns all reports sorted by given criteria/s
        /// </summary>
        [HttpGet("sortby")]
        [Authorization(new string[] { Constants.Admin })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<ReportResponseModel>>))]
        public async Task<IActionResult> SortBy([FromQuery] ReportSortRequestModel model)
        {
            var result = await this.reportService.OrderByAsync(model);

            return this.Ok(result);
        }
    }
}