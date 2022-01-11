namespace Forum.Service.Common.Message
{
    public static class Message
    {
        public static class ExceptionMessages
        {
            //Exception messages that works with entities
            public const string DOESNT_EXIST = "Doesn't exist such a {0}";
            public const string Cannot_Delete_HasRelations = "{0} has {1} cannot delete it!";
            public const string No_Entities = "No entities in collection";
            public const string Already_Reported = "Cannot report {0} more than once!";

            //Exception messages that works with User/Authentication
            public const string Unauthorized = "Invalid authentication credentials for the requested!";
            public const string Cannot_Report_YourSelf = "It is not possible to report your self!";
            public const string User_Not_Found = "Not exist such user!";
            public const string Invalid_Email = "Use correct e-mail for login!";
            public const string Already_Exist = "{0} already exist";
            public const string Invalid_Password = "Invalid password!";
            public const string Invalid_Credentials = "You do not have the enough permission for the operation!";
            public const string Already_Verified = "Already verified!";
            
        }

        public static class Constants
        {
            public const string User = "User";
            public const string Report = "Report";
            public const string Post_Report = "Post Report";
            public const string Comment_Report = "Comment Report";
            public const string Report_Type = "Report type";
            public const string Category = "Category";
            public const string Post = "Post";
            public const string Comment = "Comment";
            public const string Post_Like = "Post like";
            public const string Comment_Like = "Comment like";
            public const string Likes = "likes";
            public const string Reports = "reports";
            public const string Comments = "comments";
            public const string Posts = "posts";
            public const string Users = "users";
            public const string Categories = "categories";
            public const string Post_Reports = "post reports";
            public const string Comment_Reports = "comment reports";
            public const string Admin = "Admin";
            public const string Pending = "Pending";
            public const string Blocked = "Blocked";
            public const int User_Id = 2;
            public const int Blocked_Id = 3;
            public const int Pending_Id = 4;
            public const string Default_Avatar = "https://res.cloudinary.com/ddipdwbtm/image/upload/v1638532559/Default_Avatar_csjlik.png";
        }

        public static class ExternalProvider
        {
            //Email sender -> profile credentials
            public const string Forum_Mail = "account_alphaforum@abv.bg";
            public const string Forum_Mail_Password = "passwordQ1!";
            public const string Mail_Subject = "Alpha Forum account verification.";
            public const string From_Title = "Alpha forum";
            public const string To_Title = "To user";
            public const string SMTP_Server = "smtp.abv.bg";
            public const int SMTP_Port = 465;
            //Cloudinary -> profile credentials
            public const string Cloudinary_Username = "ddipdwbtm";
            public const string Cloudinary_ApiKey = "365346936914384";
            public const string Cloudinary_ApiSecret = "QdlBwkLV50Bec4ghX61rkgcAdvU";
        }

        public static class ResponseMessages
        {
            public const string Entity_Create_Succeed = "{0} was successfully created!";
            public const string Entity_Delete_Succeed = "{0} was successfully deleted!";
            public const string Entity_Edit_Succeed = "{0} was successfully edited!";
            public const string Entity_GetAll_Succeed = "Successfully got all {0}!";
            public const string Entity_Get_Succeed = "Successfully got {0}!";
            public const string User_Block_Succeed = "Successfully blocked {0}!";
            public const string User_UnBlock_Succeed = "Successfully Unblocked {0}!";

            public const string Logout_Suceed = "Succsessfully logout";
            public const string Login_Suceed = "Successfully loged in!";
            public const string Check_Email_For_Verification = "Please check your email for verification link.";
            public const string Email_Verification_Succeed = "{0} was verified!";
            public const string Send_Mail_Succeed = "Email was successfully send!";
        }
    }
}