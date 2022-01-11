//Local
using Forum.Models.Pagination;
using Forum.Models.Request.CommentReport;
using Forum.Models.Response;
using Forum.Models.Response.PostReport;
using Forum.Service;
using Forum.Service.Common.Extensions;
using Forum.WebApi.Helpers;
//Nuget packets
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//Public
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentReportsController : ControllerBase
    {
        private readonly ICommentReportService reportsService;

        public CommentReportsController(ICommentReportService reportsService)
        {
            this.reportsService = reportsService;
        }

        /// <summary>
        /// Returns all exsting comment reports
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<CommentReportResponseModel>>))]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> GetAll([FromQuery]PaginationRequestModel model) 
        {
            var response = await this.reportsService.GetAll(model);

            return this.Ok(response);
        }

        /// <summary>
        /// Creates comment report
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Create(CreateCommentReportRequestModel model)
        {
            var response = await this.reportsService.CreateAsync(model);

            return this.Ok(response);
        }

        /// <summary>
        /// Deletes comment report
        /// </summary>
        [HttpDelete("{commentId}/{reportId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Delete(long commentId, long reportId)
        {
            var response = await this.reportsService.DeleteAsync(commentId, reportId);

            if (!response.IsSuccess)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }
    }
}