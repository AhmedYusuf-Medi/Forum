using Forum.Models.Pagination;

namespace Forum.Models.Request.Post
{
    public class PostFilterRequestModel : PostSortRequestModel
    {
        public string Category { get; set; }

        public string Username { get; set; }

        public long? UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}