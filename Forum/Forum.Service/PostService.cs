//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Pagination;
using Forum.Models.Request.Post;
using Forum.Models.Response;
using Forum.Models.Response.Post;
using Forum.Service.Common.Extensions;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Linq;
using System.Threading.Tasks;
//Static internals
using static Forum.Service.Common.Extensions.Validator;
using static Forum.Service.Common.Message.Message;

namespace Forum.Service
{
    public class PostService : IPostService
    {
        private readonly ForumDbContext db;
        private readonly ICloudinaryService cloudinaryService;

        public PostService(ForumDbContext db, ICloudinaryService cloudinaryService)
        {
            this.db = db;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<long> GetCountAsync()
        {
            return await this.db.Posts.CountAsync();
        }

        public async Task<long> GetLikesCountAsync(long postId)
        {
            var post = await this.db.Posts.Where(p => p.Id == postId)
                .Select(p => new Post()
                {
                    Id = p.Id,
                    Likes = p.Likes
                })
                .FirstOrDefaultAsync();

            return post.Likes.Count();
        }

        public async Task<long> GetCommentsCountAsync(long postId)
        {
            var post = await this.db.Posts.Where(p => p.Id == postId)
                .Select(p => new Post()
                {
                    Id = p.Id,
                    Comments = p.Comments
                })
                .FirstOrDefaultAsync();

            return post.Comments.Count();
        }

        public async Task<Response<PostResponseModel>> GetByIdAsync(long id)
        {
            PostResponseModel post = await PostQueries.GetPostResponseById(id, this.db);

            var response = new Response<PostResponseModel>();
            response.Payload = post;

            ValidateForNull(response, ResponseMessages.Entity_Get_Succeed, Constants.Post);

            return response;
        }

        public async Task<Response<Paginate<PostResponseModel>>> GetAllAsync(PaginationRequestModel request)
        {
            var posts = PostQueries.GetAllPosts(this.db.Posts).AsQueryable();

            var paginatedPosts = await Paginate<PostResponseModel>.ToPaginatedCollection(posts, request.Page, request.PerPage);

            var response = new Response<Paginate<PostResponseModel>>();
            response.Payload = paginatedPosts;
            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Posts);

            return response;
        }

        public async Task<InfoResponse> CreateAsync(CreatePostRequestModel model)
        {
            var response = new InfoResponse();

            await ForeignKeyValidations.CheckUser(model.UserId, this.db,
                string.Format(ExceptionMessages.DOESNT_EXIST, Constants.User));
            await ForeignKeyValidations.CheckCategory(model.CategoryId, this.db,
                string.Format(ExceptionMessages.DOESNT_EXIST, Constants.Category));

            var post = Mapper.ToPost(model);

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

                post.PicturePath = uploadResults[0];
                post.PictureId = uploadResults[1];
            }

            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = string.Format(ResponseMessages.Entity_Create_Succeed, Constants.Post);

            return response;
        }

        public async Task<InfoResponse> DeleteAsync(long id, long? userId)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);

            var response = new InfoResponse();

            ValidateForNull(post, response, ResponseMessages.Entity_Delete_Succeed, Constants.Post);

            if (userId.HasValue)
            {
                if (response.IsSuccess)
                {
                    CheckIsOwner(post.UserId, userId, response);
                }
            }

            if (response.IsSuccess)
            {
                this.db.Posts.Remove(post);
                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<InfoResponse> EditAsync(long id, long userId, EditPostRequestModel model)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);

            var response = new InfoResponse();

            ValidateForNull(post, response, ResponseMessages.Entity_Edit_Succeed, Constants.Post);

            if (response.IsSuccess)
            {
                CheckIsOwner(post.UserId, userId, response);
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
                    if (!string.IsNullOrEmpty(post.PictureId))
                    {
                        await this.cloudinaryService.DeleteImageAsync(post.PictureId);
                    }

                    var fileName = model.Picture.FileName;
                    var uploadResults = await this.cloudinaryService.UploadPictureAsync(model.Picture, fileName, user.Username);

                    post.PicturePath = uploadResults[0];
                    post.PictureId = uploadResults[1];
                }

                if (model.CategoryId.HasValue)
                {
                    await ForeignKeyValidations.CheckCategory((long)model.CategoryId, this.db,
                        string.Format(ExceptionMessages.DOESNT_EXIST, Constants.Category));
                    post.CategoryId = (long)model.CategoryId;

                    return response;
                }

                if (DataValidations.IsStringPropertyValid(model.Title, post.Title))
                {
                    post.Title = model.Title;
                }

                if (DataValidations.IsStringPropertyValid(model.Description, post.Description))
                {
                    post.Description = model.Description;
                }

                await this.db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response<Paginate<PostResponseModel>>> OrderByAsync(PostSortRequestModel model)
        {
            var posts = this.db.Posts.AsQueryable();

            var response = new Response<Paginate<PostResponseModel>>();
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Posts);
            response.IsSuccess = true;

            if (model.Top.HasValue)
            {
                posts = posts.Take((int)model.Top);
            }

            var sortedPosts = posts.OrderByDescending(p => 1);

            if (CriteriaValidations.BySingleCriteria(model.MostRecently))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.CreatedOn);
            }

            if (CriteriaValidations.BySingleCriteria(model.MostCommented))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Comments.Count);
            }

            if (CriteriaValidations.BySingleCriteria(model.MostLiked))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Likes.Count);
            }

            if(CriteriaValidations.BySingleCriteria(model.TitleDes))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Title);
            }

            if (CriteriaValidations.BySingleCriteria(model.TitleAsc))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Title);
            }

            var paginatedPosts = await Paginate<PostResponseModel>
                .ToPaginatedCollection(PostQueries.GetAllPosts(sortedPosts) , model.Page, model.PerPage);

            response.Payload = paginatedPosts;

            return response;
        }

        public async Task<Response<Paginate<PostResponseModel>>> FilterAsync(PostFilterRequestModel model)
        {
            var posts = this.db.Posts.AsQueryable();

            var response = new Response<Paginate<PostResponseModel>>();
            response.Message = string.Format(ResponseMessages.Entity_GetAll_Succeed, Constants.Posts);
            response.IsSuccess = true;

            if (CriteriaValidations.BySingleCriteria(model.Category))
            {
                posts = posts.Where(p => p.Category.Name.ToLower().Contains(model.Category.ToLower()));
            }

            if (CriteriaValidations.BySingleCriteria(model.Username))
            {
                posts = posts.Where(p => p.User.Username == model.Username);
            }

            if (CriteriaValidations.BySingleNullableId(model.UserId))
            {
                posts = posts.Where(p => p.UserId == model.UserId);
            }

            if (CriteriaValidations.BySingleCriteria(model.Title))
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(model.Title.ToLower()));
            }

            if (CriteriaValidations.BySingleCriteria(model.Description))
            {
                posts = posts.Where(p => p.Description.ToLower().Contains(model.Description.ToLower()));
            }

            var sortedPosts = posts.OrderByDescending(p => 1);

            if (CriteriaValidations.BySingleCriteria(model.MostRecently))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.CreatedOn);
            }

            if (CriteriaValidations.BySingleCriteria(model.MostCommented))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Comments.Count);
            }

            if (CriteriaValidations.BySingleCriteria(model.MostLiked))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Likes.Count);
            }

            if (CriteriaValidations.BySingleCriteria(model.TitleAsc))
            {
                sortedPosts = sortedPosts.ThenBy(p => p.Title);
            }

            if (CriteriaValidations.BySingleCriteria(model.TitleDes))
            {
                sortedPosts = sortedPosts.ThenByDescending(p => p.Title);
            }

            var paginatedPosts = await Paginate<PostResponseModel>
                .ToPaginatedCollection(PostQueries.GetAllPosts(sortedPosts), model.Page, model.PerPage);
            response.Payload = paginatedPosts;

            return response;
        }
    }
}