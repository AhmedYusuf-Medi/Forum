//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Request.PostLike;
using Forum.Models.Response;
using Forum.Service.Common.Extensions;
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
    public class PostLikeService : IPostLikeService
    {
        private readonly ForumDbContext db;

        public PostLikeService(ForumDbContext db)
        {
            this.db = db;
        }
        public async Task<InfoResponse> CreateAsync(PostLikeRequestModel postLikeRequest)
        {
            var response = await this.ManipulateLikeStateAsync(postLikeRequest, new InfoResponse());

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id)
        {
            var response = new InfoResponse();
            var forDelete = await this.db.Post_Likes.FirstOrDefaultAsync(u => u.Id == id);

            Validator.ValidateForNull(forDelete, response, ResponseMessages.Entity_Delete_Succeed, Constants.Post_Like);

            if (response.IsSuccess)
            {
                this.db.Post_Likes.Remove(forDelete);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        /// <summary>
        /// It manipulates like state by the current
        /// if the like doesn't exist it creates it ,else if the like is crated he deletes it
        /// and for the final case if it's created but deleted later undeletes/revives it.
        /// </summary>
        private async Task<InfoResponse> ManipulateLikeStateAsync(PostLikeRequestModel postLikeRequest, InfoResponse response)
        {
            var deletedPostLike = await this.db.Post_Likes
                                            .Where(pl => postLikeRequest.PostId == pl.PostId
                                                      && postLikeRequest.UserId == pl.UserId)
                                            .IgnoreQueryFilters()
                                            .FirstOrDefaultAsync();
            if (deletedPostLike == null)
            {
                var newPostLike = new Post_Like()
                {
                    UserId = postLikeRequest.UserId,
                    PostId = postLikeRequest.PostId
                };

                await this.db.Post_Likes.AddAsync(newPostLike);
                await this.db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Post_Like);

                return response;
            }
            else if (!deletedPostLike.IsDeleted)
            {
                return await this.DeleteAsync(deletedPostLike.Id);
            }

            this.db.Undelete(deletedPostLike);

            await this.db.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Post_Like);

            return response;
        }
    }
}