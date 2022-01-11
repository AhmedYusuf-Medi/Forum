namespace Forum.Models.Response.PostReport
{
    public class CommentReportResponseModel
    {
        public CommentReportResponseModel(string receiver, string sender, long commentId, string type)
        {
            this.Receiver = receiver;
            this.Sender = sender;
            this.CommentId = commentId;
            this.Type = type;
        }

        public string Receiver { get; set; }
        public string Sender { get; set; }
        public long CommentId { get; set; }
        public string Type { get; set; }
    }
}