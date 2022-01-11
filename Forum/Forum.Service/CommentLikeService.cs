//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Request.CommentLike;
using Forum.Models.Response;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Linq;
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;
using static Forum.Service.Common.Extensions.Validator;

namespace Forum.Service
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly ForumDbContext db;

        public CommentLikeService(ForumDbContext db)
        {
            this.db = db;
        }
        public async Task<InfoResponse> CreateAsync(CommentLikeRequestModel commentLikeRequest)
        {
            var response = await this.ManipulateLikeStateAsync(commentLikeRequest, new InfoResponse());

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id)
        {
            var response = new InfoResponse();
            var forDelete = await this.db.Comment_Likes.FirstOrDefaultAsync(u => u.Id == id);

            ValidateForNull(forDelete, response, ResponseMessages.Entity_Delete_Succeed, Constants.Comment_Like);

            if (response.IsSuccess)
            {
                this.db.Comment_Likes.Remove(forDelete);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        /// <summary>
        /// It manipulates like state by the current
        /// if the like doesn't exist it creates it ,else if the like is crated he deletes it
        /// and for the final case if it's created but deleted later undeletes/revives it.
        /// </summary>
        private async Task<InfoResponse> ManipulateLikeStateAsync(CommentLikeRequestModel commentLikeRequest, InfoResponse response)
        {
            var deletedCommentLike = await this.db.Comment_Likes
                                            .Where(cl => commentLikeRequest.CommentId == cl.CommentId
                                                      && commentLikeRequest.UserId == cl.UserId)
                                            .IgnoreQueryFilters()
                                            .FirstOrDefaultAsync();

            if (deletedCommentLike == null)
            {
                var newCommentLike = new Comment_Like()
                {
                    UserId = commentLikeRequest.UserId,
                    CommentId = commentLikeRequest.CommentId
                };

                await this.db.Comment_Likes.AddAsync(newCommentLike);
                await this.db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Comment_Like);
            }
            else if (!deletedCommentLike.IsDeleted)
            {
                response = await this.DeleteAsync(deletedCommentLike.Id);
            }
            else
            {
                this.db.Undelete(deletedCommentLike);

                await this.db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Comment_Like);
            }

            return response;
        }
    }
}