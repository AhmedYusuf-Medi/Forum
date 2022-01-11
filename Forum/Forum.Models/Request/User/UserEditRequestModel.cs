using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.User
{
    public class UserEditRequestModel
    {
        [StringLength(20, MinimumLength = 2, ErrorMessage = " must be between {2} and {1}.")]
        public string Username { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = " must be between {2} and {1}.")]
        public string DisplayName { get; set; }

        [EmailAddress(ErrorMessage = " should be valid.")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = " must be at least 8 symbols should contain capital letter, digit and special symbol (+, -, *, &, ^, …)!")]
        public string Password { get; set; }

        public IFormFile Avatar { get; set; }
    }
}