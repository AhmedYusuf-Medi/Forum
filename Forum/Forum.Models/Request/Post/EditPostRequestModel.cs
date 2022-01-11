using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.Post
{
    public class EditPostRequestModel
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Title must be between {2} and {1}")]
        public string Title { get; set; }

        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1}")]
        public string Description { get; set; }

        public IFormFile Picture { get; set; }

        [Range(1, long.MaxValue)]
        public long? CategoryId { get; set; }
    }
}