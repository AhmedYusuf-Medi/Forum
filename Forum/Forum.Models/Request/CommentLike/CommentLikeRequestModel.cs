using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.CommentLike
{
    public class CommentLikeRequestModel
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Range(1, long.MaxValue)]
        public long CommentId { get; set; }
    }
}