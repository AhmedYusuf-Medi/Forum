//Local
using Forum.Data;
using Forum.Models.Pagination;
using Forum.Models.Request.User;
using Forum.Models.Response;
using Forum.Models.Response.User;
using Forum.Service.Common.Extensions;
using Forum.Service.Common.Message;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Linq;
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.Service
{
    public class UserService : IUserService
    {
        private readonly ForumDbContext db;

        public UserService(ForumDbContext db)
        {
            this.db = db;
        }

        public async Task<Response<Paginate<UserResponseModel>>> GetAllAsync(PaginationRequestModel request)
        {
            var result = this.db.Users
                    .Select(user => new UserResponseModel()
                    {
                        Id = user.Id,
                        UserName = user.Username,
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Password = user.Password,
                        PicturePath = user.PicturePath,
                        Role = user.Role.Type,
                        PostsCount = user.Posts.Count()
                    })
                    .AsQueryable();

            var payload = await Paginate<UserResponseModel>.ToPaginatedCollection(result, request.Page, request.PerPage);

            var response = new Response<Paginate<UserResponseModel>>();
            response.Payload = payload;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, "users");

            return response;
        }

        public async Task<Response<UserResponseModel>> GetByIdAsync(long id)
        {
            var response = new Response<UserResponseModel>();
            var result = await UserByIdAsync(id);
            response.Payload = result;

            Validator.ValidateForNull(response, ResponseMessages.Entity_Get_Succeed, Constants.User);

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id)
        {
            var response = new InfoResponse();
            var forDelete = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            Validator.ValidateForNull(forDelete, response, ResponseMessages.Entity_Delete_Succeed, Constants.User);

            if (response.IsSuccess)
            {
                this.db.Users.Remove(forDelete);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<long> GetCountAsync()
        {
            var result = await this.db.Users.CountAsync();
            return result;
        }

        public async Task<InfoResponse> BlockAsync(long id)
        {
            var responce = await SetRoleAsync(id, Constants.Blocked_Id, ResponseMessages.User_Block_Succeed);
            return responce;
        }

        public async Task<InfoResponse> UnBlockAsync(long id)
        {
            var responce = await SetRoleAsync(id, Constants.User_Id, ResponseMessages.User_UnBlock_Succeed);
            return responce;
        }

        public async Task<Response<Paginate<UserResponseModel>>> SearchByAsync(UserSearchRequestModel model)
        {
            var query = this.db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(model.Username))
            {
                query = query.Where(u => u.Username.Contains(model.Username));
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                query = query.Where(u => u.Email.Contains(model.Email));
            }

            if (!string.IsNullOrEmpty(model.DisplayName))
            {
                query = query.Where(u => u.DisplayName.Contains(model.DisplayName));
            }

            var filtered = query.Select(user => new UserResponseModel()
            {
                Id = user.Id,
                UserName = user.Username,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Password = user.Password,
                PicturePath = user.PicturePath,
                Role = user.Role.Type,
                PostsCount = user.Posts.Count()
            });

            var payload = await Paginate<UserResponseModel>.ToPaginatedCollection(filtered, model.Page, model.PerPage);

            var response = new Response<Paginate<UserResponseModel>>();
            response.Payload = payload;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Users);

            return response;
        }

        private async Task<InfoResponse> SetRoleAsync(long id, int roleId, string msg)
        {
            var response = new InfoResponse();
            var forBlock = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            Validator.ValidateForNull(forBlock, response, msg, Constants.User);

            if (response.IsSuccess)
            {
                forBlock.RoleId = roleId;
                await db.SaveChangesAsync();
            }
            return response;
        }

        private async Task<UserResponseModel> UserByIdAsync(long id)
        {
            var result = await this.db.Users.Where(u => u.Id == id)
                .Select(user => new UserResponseModel()
                {
                    Id = user.Id,
                    UserName = user.Username,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Password = user.Password,
                    PicturePath = user.PicturePath,
                    Role = user.Role.Type,
                    Code = user.Code,
                    PostsCount = user.Posts.Count()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}