using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.Category
{
    public class CategoryRequestModel
    {
        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "Category must be between {2} and {1}")]
        public string Name { get; set; }
    }
}