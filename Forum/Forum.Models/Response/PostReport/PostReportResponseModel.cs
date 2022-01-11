namespace Forum.Models.Response.PostReport
{
    public class PostReportResponseModel
    {
        public PostReportResponseModel(string receiver, string sender, long postId, string type)
        {
            this.Receiver = receiver;
            this.Sender = sender;
            this.PostId = postId;
            this.Type = type;
        }

        public string Receiver { get; set; }
        public string Sender { get; set; }
        public long PostId { get; set; }
        public string Type { get; set; }
    }
}