//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Response.Post;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Service.Common.Extensions
{
    public static class PostQueries
    {
        public static Func<IQueryable<Post>, IQueryable<PostResponseModel>> GetAllPosts
            => (IQueryable<Post> posts) =>
                posts.Select(p => new PostResponseModel
                (
                    p.Id,
                    p.Title,
                    p.Description,
                    p.PicturePath,
                    p.Category.Name,
                    p.CreatedOn,
                    p.Comments.Count(),
                    p.User.Username,
                    p.Likes.Count(),
                    p.UserId,
                    p.User.PicturePath
               ));
           

        public static async Task<PostResponseModel> GetPostResponseById(long id,ForumDbContext db)
        {
            return await db.Posts.Where(p => p.Id == id)
                  .Select(p => new PostResponseModel
                  (
                     p.Id,
                     p.Title,
                     p.Description,
                     p.PicturePath,
                     p.Category.Name,
                     p.CreatedOn,
                     p.Comments.Count(),
                     p.User.Username,
                     p.Likes.Count(),
                     p.UserId,
                     p.User.PicturePath
                   ))
                  .FirstOrDefaultAsync();
        }   
    }
}