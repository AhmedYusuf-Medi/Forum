using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    public class Post : Entity
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Likes = new HashSet<Post_Like>();
            this.Reports = new HashSet<PostReport>();
        }

        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Title must be between {2} and {1}")]
        public string Title { get; set; }

        [Required, StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1}")]
        public string Description { get; set; }

        public string PicturePath { get; set; }

        public string PictureId { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Post_Like> Likes { get; set; }

        public ICollection<PostReport> Reports { get; set; }
    }
}