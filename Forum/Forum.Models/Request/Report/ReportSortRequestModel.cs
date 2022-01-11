using Forum.Models.Pagination;

namespace Forum.Models.Request.Report
{
    public class ReportSortRequestModel : PaginationRequestModel
    {
        public string MostRecently { get; set; }
    }
}