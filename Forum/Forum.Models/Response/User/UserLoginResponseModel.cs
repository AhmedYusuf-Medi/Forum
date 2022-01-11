namespace Forum.Models.Response.User
{
    public class UserLoginResponseModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
    }
}