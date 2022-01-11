using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.Post
{
    public class CreatePostRequestModel
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Required, Range(1, long.MaxValue)]
        public long CategoryId { get; set; }
        
        [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Title must be between {2} and {1}")]
        public string Title { get; set; }

        [Required, StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1}")]
        public string Description { get; set; }

        public IFormFile Picture { get; set; }
    }
}