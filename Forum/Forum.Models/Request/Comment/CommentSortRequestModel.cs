using Forum.Models.Pagination;

namespace Forum.Models.Request.Comment
{
    public class CommentSortRequestModel : PaginationRequestModel
    {
        public string MostRecently { get; set; }

        public string MostLiked { get; set; }

        public long? PostId { get; set; }
    }
}