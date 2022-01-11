using Forum.Models.Pagination;

namespace Forum.Models.Request.Category
{
    public class CategorySortRequestModel : PaginationRequestModel
    {
        public string MostUploaded { get; set; }

        public string MostRecently { get; set; }
    }
}