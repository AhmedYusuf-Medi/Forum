using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Entities
{
    public class User : Entity
    {
        public User()
        {
            this.CommentLikes = new HashSet<Comment_Like>();
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.PostLikes = new HashSet<Post_Like>();
            this.SendReports = new HashSet<Report>();
            this.ReceivedReports = new HashSet<Report>();
        }
       
        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between {2} and {1}.")]
        public string Username { get; set; }

        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "Displayname must be between {2} and {1}.")]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email should be valid.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must be at least 8 symbols should contain capital letter, digit and special symbol (+, -, *, &, ^, …)!")]
        public string Password { get; set; }

        public string PicturePath { get; set; }

        public string PictureId { get; set; }

        public Guid? Code { get; set; }

        public long RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public ICollection<Comment_Like> CommentLikes { get; set; }
        public ICollection<Post_Like> PostLikes { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Report> SendReports { get; set; }
        public ICollection<Report> ReceivedReports { get; set; }
    }
}