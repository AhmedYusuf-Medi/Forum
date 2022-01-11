using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.Report
{
    public class CreateReportRequestModel
    {
        [Required, Range(1, long.MaxValue, ErrorMessage = "Sender Id must be between {2} and {1}")]
        public long SenderId { get; set; }

        [Required, Range(1, long.MaxValue, ErrorMessage = "Receiver Id must be between {2} and {1}")]
        public long ReceiverId { get; set; }

        [Required]
        public long ReportTypeId { get; set; }

        [Required, StringLength(250, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1}")]
        public string Description { get; set; }
    }
}