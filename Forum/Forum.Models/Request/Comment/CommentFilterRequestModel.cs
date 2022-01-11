using Forum.Models.Pagination;

namespace Forum.Models.Request.Comment
{
    public class CommentFilterRequestModel : PaginationRequestModel
    {
        public long? UserId { get; set; }

        public long? PostId { get; set; }

        public string Description { get; set; }

        public string MostRecently { get; set; }

        public string MostLiked { get; set; }
    }
}