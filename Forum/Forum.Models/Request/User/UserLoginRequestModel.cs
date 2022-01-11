using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Request.User
{
    public class UserLoginRequestModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}