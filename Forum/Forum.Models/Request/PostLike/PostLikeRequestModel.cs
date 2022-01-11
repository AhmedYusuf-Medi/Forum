using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.PostLike
{
    public class PostLikeRequestModel
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Range(1, long.MaxValue)]
        public long PostId { get; set; }
    }
}