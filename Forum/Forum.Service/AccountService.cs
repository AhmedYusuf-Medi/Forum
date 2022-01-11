//Local
using Forum.Data;
using Forum.Models.Request.User;
using Forum.Models.Response;
using Forum.Models.Response.User;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System;
using System.Linq;
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Extensions.Validator;
using static Forum.Service.Common.Message.Message;

namespace Forum.Service
{
    public class AccountService : IAccountService
    {
        private readonly ForumDbContext db;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IMailService mailService;

        public AccountService(ForumDbContext db,
                            ICloudinaryService cloudinaryService,
                            IMailService mailService)
        {
            this.db = db;
            this.cloudinaryService = cloudinaryService;
            this.mailService = mailService;
        }

        public async Task<InfoResponse> RegisterUserAsync(UserRegisterRequestModel user)
        {
            var response = new InfoResponse();

            var checkForExistEmail = await this.db.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();

            if (checkForExistEmail != null)
            {
                response.Message = string.Format(ExceptionMessages.Already_Exist, Constants.User);
                return response;
            }

            var checkForExistUserName = await this.db.Users.Where(u => u.Username == user.UserName).FirstOrDefaultAsync();

            if (checkForExistUserName != null)
            {
                response.Message = string.Format(ExceptionMessages.Already_Exist, Constants.User);
                return response;
            }

            var code = new Guid();
            code = Guid.NewGuid();

            var newUser = Mapper.ToUser(user);
            newUser.PicturePath = Constants.Default_Avatar;
            newUser.Code = code;
            newUser.RoleId = Constants.Pending_Id;
            await this.db.Users.AddAsync(newUser);
            await this.db.SaveChangesAsync();

            await this.mailService.SendMailAsync(user.Email, code);

            response.IsSuccess = true;
            response.Message = ResponseMessages.Check_Email_For_Verification;
            return response;
        }

        public async Task<InfoResponse> EditUserAsync(long id, UserEditRequestModel user)
        {
            var response = new InfoResponse();

            var forUpdate = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            ValidateForNull(forUpdate, response, ResponseMessages.Entity_Edit_Succeed, Constants.User);

            if (response.IsSuccess)
            {

                if (user.Avatar != null)
                {
                    if (!string.IsNullOrEmpty(forUpdate.PictureId))
                    {
                        await this.cloudinaryService.DeleteImageAsync(forUpdate.PictureId);
                    }

                    var fileName = user.Avatar.FileName;
                    var uploadResults = await this.cloudinaryService.UploadPictureAsync(user.Avatar, fileName, forUpdate.Username);

                    forUpdate.PicturePath = uploadResults[0];
                    forUpdate.PictureId = uploadResults[1];
                }
                if (DataValidations.IsStringPropertyValid(user.DisplayName, forUpdate.DisplayName))
                {
                    forUpdate.DisplayName = user.DisplayName;
                }
                if (DataValidations.IsStringPropertyValid(user.Username, forUpdate.Username))
                {
                    forUpdate.Username = user.Username;
                }
                if (DataValidations.IsStringPropertyValid(user.Password, forUpdate.Password))
                {
                    forUpdate.Password = user.Password;
                }
                if (DataValidations.IsStringPropertyValid(user.Email, forUpdate.Email))
                {
                    forUpdate.Email = user.Email;
                }

                await this.db.SaveChangesAsync();

            }
            return response;
        }

        public async Task<Response<UserLoginResponseModel>> LoginAsync(UserLoginRequestModel userLogin)
        {
            var responce = new Response<UserLoginResponseModel>();

            var user = await this.db.Users
                               .Where(u => u.Email == userLogin.Email && u.Password == userLogin.Password)
                               .Select(u => new UserLoginResponseModel()
                               {
                                   Id = u.Id,
                                   Username = u.Username,
                                   Role = u.Role.Type,
                                   Avatar = u.PicturePath
                               })
                               .FirstOrDefaultAsync();

            ValidateForNull(user, responce, ResponseMessages.Login_Suceed, Constants.User);

            responce.Payload = user;
            return responce;
        }


        public async Task<InfoResponse> VerificationAsync(string email, Guid code)
        {
            var user = await this.db.Users
                            .Where(u => u.Email == email && u.Code == code)
                            .FirstOrDefaultAsync();

            var response = new InfoResponse();

            ValidateForNull(user, response, ResponseMessages.Email_Verification_Succeed, email);

            if (response.IsSuccess)
            {
                if (user.RoleId == Constants.User_Id)
                {
                    response.Message = ExceptionMessages.Already_Verified;
                }
                else
                {
                    user.RoleId = Constants.User_Id;
                    await this.db.SaveChangesAsync();
                }
            }

            return response;
        }
    }
}