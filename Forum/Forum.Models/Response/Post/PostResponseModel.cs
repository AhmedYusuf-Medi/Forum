using System;

namespace Forum.Models.Response.Post
{
    public class PostResponseModel
    {
        public PostResponseModel(long id, string title, string description,
                                string picturePath, string category,
                                DateTime createdOn, long commentCount,
                                string username, long likesCount,
                                long userId, string avatarPath)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Category = category;
            this.PicturePath = picturePath;
            this.CreatedOn = createdOn;
            this.CommentCount = commentCount;
            this.Username = username;
            this.LikesCount = likesCount;
            this.UserId = userId;
            this.Avatar = avatarPath;
        }

        public long Id { get; set; }

        public long UserId { get; set; }

        public string Username { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PicturePath { get; set; }

        public string Category { get; set; }

        public long CommentCount { get; set; }

        public long LikesCount { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Avatar { get; set; }
    }
}