using Forum.Models.Request.User;
using Forum.Models.Response;
using Forum.Models.Response.User;
using System;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IAccountService
    {
        Task<InfoResponse> EditUserAsync(long id, UserEditRequestModel user);

        Task<Response<UserLoginResponseModel>> LoginAsync(UserLoginRequestModel userLogin);

        Task<InfoResponse> RegisterUserAsync(UserRegisterRequestModel user);

        Task<InfoResponse> VerificationAsync(string email, Guid code);
    }
}