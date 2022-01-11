//Local
using Forum.Models.Pagination;
using Forum.Models.Request.PostReport;
using Forum.Models.Response;
using Forum.Models.Response.PostReport;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
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
    public class PostReportsController : ControllerBase
    {
        private readonly IPostReportService reportsService;

        public PostReportsController(IPostReportService reportsService)
        {
            this.reportsService = reportsService;
        }

        /// <summary>
        /// Returns all exsting post reports
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Paginate<PostReportResponseModel>>))]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> GetAll([FromQuery]PaginationRequestModel model) 
        {
            var response = await this.reportsService.GetAll(model);

            return this.Ok(response);
        }

        /// <summary>
        /// Creates post report
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [Authorization(new string[] { Constants.User, Constants.Admin })]
        public async Task<IActionResult> Create(CreatePostReportRequestModel model)
        {
            var response = await this.reportsService.CreateAsync(model);

            return this.Ok(response);
        }

        /// <summary>
        /// Deletes post report
        /// </summary>
        [HttpDelete("{postId}/{reportId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(InfoResponse))]
        [Authorization(new string[] { Constants.Admin })]
        public async Task<IActionResult> Delete(long postId, long reportId)
        {
            var response = await this.reportsService.DeleteAsync(postId, reportId);

            if (!response.IsSuccess)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }
    }
}