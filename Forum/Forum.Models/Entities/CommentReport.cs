using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    [Table("Comment_Reports")]
    public class CommentReport : EntityWithoutKey
    {
        [Key]
        public long ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public Report Report { get; set; }

        [Key]
        public long CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
    }
}