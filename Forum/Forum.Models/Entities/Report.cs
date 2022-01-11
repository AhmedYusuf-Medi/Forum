using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    public class Report : Entity
    {
        public Report()
        {
            this.PostReports = new HashSet<PostReport>();
            this.CommentReports = new HashSet<CommentReport>();
        }

        [Required]
        public long SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }

        [Required]
        public long ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public User Receiver { get; set; }

        [Required]
        public long ReportTypeId { get; set; }
        [ForeignKey(nameof(ReportTypeId))]
        public ReportType ReportType { get; set; }

        [Required, StringLength(250, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1}")]
        public string Description { get; set; }

        public ICollection<PostReport> PostReports { get; set; }
        public ICollection<CommentReport> CommentReports { get; set; }
    }
}