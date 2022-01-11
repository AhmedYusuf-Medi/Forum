using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    [Table("Post_Reports")]
    public class PostReport : EntityWithoutKey
    {
        [Key]
        public long ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public Report Report { get; set; }

        [Key]
        public long PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}