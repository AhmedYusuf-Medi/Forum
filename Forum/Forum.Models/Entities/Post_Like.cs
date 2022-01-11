using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    public class Post_Like : Entity
    {
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}