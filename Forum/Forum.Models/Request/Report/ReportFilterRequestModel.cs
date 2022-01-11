using Forum.Models.Pagination;

namespace Forum.Models.Request.Report
{
    public class ReportFilterRequestModel : PaginationRequestModel
    {
        public string Receiver { get; set; }

        public string Sender { get; set; }

        public long? UserId { get; set; }
    }
}