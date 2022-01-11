//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Pagination;
using Forum.Models.Request.Comment;
using Forum.Models.Response;
using Forum.Models.Response.Comment;
using Forum.Service.Common.Extensions;
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
    public class CommentService : ICommentService
    {
        private readonly ForumDbContext db;
        private readonly ICloudinaryService cloudinaryService;

        public CommentService(ForumDbContext db, ICloudinaryService cloudinaryService)
        {
            this.db = db;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<long> GetCountAsync()
        {
           return await this.db.Comments.CountAsync();
        }

        public async Task<long> GetPostCommentsCountAsync(long postId)
        {
            var post = await this.db.Posts.Where(p => p.Id == postId)
                 .Select(p => new Post()
                 {
                     Comments = p.Comments
                 })
                 .FirstOrDefaultAsync();

            return post.Comments.Count();
        }

        public async Task<long> GetLikesCountAsync(long commentId)
        {
            var comment = await this.db.Comments.Where(c => c.Id == commentId)
                 .Select(c => new Comment()
                 {
                     Likes = c.Likes
                 })
                 .FirstOrDefaultAsync();

            return comment.Likes.Count();
        }

        public async Task<Response<CommentResponseModel>> GetByIdAsync(long id)
        {
            CommentResponseModel comment = await CommentQueries.GetCommentResponseById(id, this.db);

            var response = new Response<CommentResponseModel>();
            response.Payload = comment;

            ValidateForNull(response, ResponseMessages.Entity_Get_Succeed, Constants.Comment);

            return response;
        }

        public async Task<Response<Paginate<CommentResponseModel>>> GetAllAsync(PaginationRequestModel request)
        {
            var comments = CommentQueries.GetAllComments(this.db.Comments);

            var paginatedComments = await Paginate<CommentResponseModel>.
                ToPaginatedCollection(comments, request.Page, request.PerPage);

            var response = new Response<Paginate<CommentResponseModel>>();
            response.Payload = paginatedComments;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Comments);

            return response;
        }

        public async Task<InfoResponse> CreateAsync(CreateCommentRequestModel model)
        {
            var response = new InfoResponse();

            await ForeignKeyValidations.CheckUser(model.UserId, this.db,
                string.Format(ExceptionMessages.DOESNT_EXIST, Constants.User));
            await ForeignKeyValidations.CheckPost(model.PostId, this.db,
                string.Format(ExceptionMessages.DOESNT_EXIST, Constants.Post));

            var comment = Mapper.ToComment(model);

            if (model.Picture != null)
            {
                var user = await this.db.Users
                    .Where(u => u.Id == model.UserId)
                    .Select(u => new User()
                    {
                        Username = u.Username
                    })
                    .FirstOrDefaultAsync();

                var fileName = model.Picture.FileName;
                var uploadResults = await this.cloudinaryService.UploadPictureAsync(model.Picture, fileName, user.Username);

                comment.PicturePath = uploadResults[0];
                comment.PictureId = uploadResults[1];
            }

            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();
            
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Comment);

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id, long? userId)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(c => c.Id == id);

            var response = new InfoResponse();

            ValidateForNull(comment, response, ResponseMessages.Entity_Delete_Succeed, Constants.Comment);

            if (userId.HasValue)
            {
                if (response.IsSuccess)
                {
                    CheckIsOwner(comment.UserId, userId, response);
                }
            }

            if (response.IsSuccess)
            {
                this.db.Comments.Remove(comment);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<InfoResponse> EditAsync(long id, long userId, EditCommentRequestModel model)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(c => c.Id == id);

            var response = new InfoResponse();

            ValidateForNull(comment, response, ResponseMessages.Entity_Edit_Succeed, Constants.Comment);

            if (response.IsSuccess)
            {
                CheckIsOwner(comment.UserId, userId, response);
            }

            if (response.IsSuccess)
            {
                var user = await this.db.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new User()
                    {
                        Username = u.Username
                    })
                    .FirstOrDefaultAsync();

                if (model.Picture != null)
                {
                    if (!string.IsNullOrEmpty(comment.PictureId))
                    {
                        await this.cloudinaryService.DeleteImageAsync(comment.PictureId);
                    }

                    var fileName = model.Picture.FileName;
                    var uploadResults = await this.cloudinaryService.UploadPictureAsync(model.Picture, fileName, user.Username);

                    comment.PicturePath = uploadResults[0];
                    comment.PictureId = uploadResults[1];
                }

                if (DataValidations.IsStringPropertyValid(model.Description, comment.Description))
                {
                    comment.Description = model.Description;
                }

                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response<Paginate<CommentResponseModel>>> OrderByAsync(CommentSortRequestModel model)
        {
            var postComments = this.db.Comments.AsQueryable();

            if (CriteriaValidations.BySingleNullableId(model.PostId))
            {
                postComments = postComments.Where(c => c.PostId == model.PostId);
            }

            var sortedPostComments = postComments.OrderByDescending(c => 1);

            if (CriteriaValidations.BySingleCriteria(model.MostRecently))
            {
                sortedPostComments = sortedPostComments
                   .ThenByDescending(c => c.CreatedOn);
            }
            else if (CriteriaValidations.BySingleCriteria(model.MostLiked))
            {
                sortedPostComments = sortedPostComments
                   .ThenByDescending(c => c.Likes.Count());
            }

            var paginatedComments = await Paginate<CommentResponseModel>
                .ToPaginatedCollection(CommentQueries.GetAllComments(sortedPostComments), model.Page, model.PerPage);

            var response = new Response<Paginate<CommentResponseModel>>();
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Comments);
            response.IsSuccess = true;
            response.Payload = paginatedComments;

            return response;
        }

        public async Task<Response<Paginate<CommentResponseModel>>> FilterAsync(CommentFilterRequestModel model)
        {
            var comments = this.db.Comments.AsQueryable();

            if (CriteriaValidations.BySingleNullableId(model.UserId))
            {
                comments = comments.Where(c => c.UserId == model.UserId);
            }

            if (CriteriaValidations.BySingleNullableId(model.PostId))
            {
                comments = comments.Where(c => c.PostId == model.PostId);
            }

            if (CriteriaValidations.BySingleCriteria(model.Description))
            {
                comments = comments.Where(c => c.Description.ToLower().Contains(model.Description));
            }

            var sortedComments = comments.OrderByDescending(c => 1);

            if (CriteriaValidations.BySingleCriteria(model.MostRecently))
            {
                sortedComments = sortedComments.ThenByDescending(c => c.CreatedOn);
            }

            if(CriteriaValidations.BySingleCriteria(model.MostLiked))
            {
                sortedComments = sortedComments.ThenByDescending(c => c.Likes.Count);
            }

            var paginatedComments = await Paginate<CommentResponseModel>
                .ToPaginatedCollection(CommentQueries.GetAllComments(comments), model.Page, model.PerPage);

            var response = new Response<Paginate<CommentResponseModel>>();
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Comments);
            response.IsSuccess = true;
            response.Payload = paginatedComments;

            return response;
        }
    }
}