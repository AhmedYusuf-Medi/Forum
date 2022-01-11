using Forum.Models.Request.Comment;
using Forum.Models.Response.Post;

namespace Forum.WebMVC.Models.Posts
{
    public class DisplayPostCommentsViewModel
    {
        public int Page { get; set; }

        public PostResponseModel Post { get; set; }

        public CreateCommentRequestModel Comment { get; set; }
    }
}
