using Forum.Models.Request.Report;

namespace Forum.Models.Request.CommentReport
{
    public class CreateCommentReportRequestModel : CreateReportRequestModel
    {
        public long CommentId { get; set; }
    }
}