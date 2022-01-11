using System;

namespace Forum.Models.Response.Comment
{
    public class CommentResponseModel
    {
        public CommentResponseModel(string username, string postTitle,
                                   string description, string picPath,
                                   long likes, DateTime createdOn,
                                   long userId, long postId,
                                   string avatar, long id)
        {
            this.Id = id;
            this.Username = username;
            this.PostTitle = postTitle;
            this.Description = description;
            this.PicturePath = picPath;
            this.Likes = likes;
            this.CreatedOn = createdOn;
            this.PostId = postId;
            this.UserId = userId;
            this.Avatar = avatar;
        }

        public long Id { get; set; }

        public long UserId { get; set; }

        //TO DO Replace with display name
        public string Username { get; set; }

        public long PostId { get; set; }

        public string PostTitle { get; set; }

        public string Description { get; set; }

        public string PicturePath { get; set; }

        public long Likes { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Avatar { get; set; }
    }
}