using Forum.Models.Request.Report;

namespace Forum.Models.Request.PostReport
{
    public class CreatePostReportRequestModel : CreateReportRequestModel
    {
        public long PostId { get; set; }
    }
}