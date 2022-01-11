using Forum.Models.Pagination;

namespace Forum.Models.Request.User
{
    public class UserSearchRequestModel : PaginationRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}