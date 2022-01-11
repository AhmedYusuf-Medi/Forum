using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    [Table("Report_Types")]
    public class ReportType : Entity
    {
        public ReportType()
        {
            this.Reports = new HashSet<Report>();
        }

        [Required, StringLength(40, MinimumLength = 2, ErrorMessage = "Report must be between {2} and {1}")]
        public string Name { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}