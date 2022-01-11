using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    public class Comment : Entity
    {
        public Comment()
        {
            this.Likes = new HashSet<Comment_Like>();
            this.Reports = new HashSet<CommentReport>();
        }

        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [Required, StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1}")]
        public string Description { get; set; }

        public string PicturePath { get; set; }

        public string PictureId { get; set; }

        public ICollection<Comment_Like> Likes { get; set; }

        public ICollection<CommentReport> Reports { get; set; }
    }
}