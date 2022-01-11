using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.User
{
    public class UserRegisterRequestModel
    {
        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "First must be between {2} and {1}.")]
        public string UserName { get; set; }

        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be between {2} and {1}.")]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email should be valid.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must be at least 8 symbols should contain capital letter, digit and special symbol (+, -, *, &, ^, …)!")]
        public string Password { get; set; }
    }
}