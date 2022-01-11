using Forum.Models.Pagination;

namespace Forum.Models.Request.Post
{
    public class PostSortRequestModel : PaginationRequestModel
    {
        public string MostRecently { get; set; }

        public string MostCommented { get; set; }

        public string MostLiked { get; set; }

        public string TitleAsc { get; set; }

        public string TitleDes { get; set; }

        public int? Top { get; set; }
    }
}