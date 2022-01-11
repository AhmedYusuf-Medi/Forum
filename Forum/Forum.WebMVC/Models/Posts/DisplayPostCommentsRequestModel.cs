using Forum.Models.Pagination;

namespace Forum.WebMVC.Models.Posts
{
    public class DisplayPostCommentsRequestModel : PaginationRequestModel
    {
        public long PostId { get; set; }
    }
}
