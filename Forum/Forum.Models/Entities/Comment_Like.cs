using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    public class Comment_Like : Entity
    {
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long CommentId { get; set; }

        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
    }
}