namespace Forum.Models.Response.Report
{
    public class ReportResponseModel
    {
        public ReportResponseModel(string sender, string receiver, string reportType, string description)
        {
            this.SenderUsername = sender;
            this.ReceiverUsername = receiver;
            this.ReportType = reportType;
            this.Description = description;
        }

        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string ReportType { get; set; }

        public string Description { get; set; }
    }
}