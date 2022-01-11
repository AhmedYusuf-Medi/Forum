//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Models.Response.Comment;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Service.Common.Extensions
{
    public abstract class CommentQueries
    {
        public static Func<IQueryable<Comment>, IQueryable<CommentResponseModel>> GetAllComments
           => (IQueryable<Comment> comments) =>
             comments.Select(c => new CommentResponseModel
             (
                c.User.Username,
                c.Post.Title,
                c.Description,
                c.PicturePath,
                c.Likes.Count(),
                c.CreatedOn,
                c.UserId,
                c.PostId,
                c.User.PicturePath,
                c.Id
              ));

        public static async Task<CommentResponseModel> GetCommentResponseById(long id, ForumDbContext db)
        {
            return await db.Comments.
                Where(c => c.Id == id)
               .Select(c => new CommentResponseModel
                      (
                        c.User.Username,
                        c.Post.Title,
                        c.Description,
                        c.PicturePath,
                        c.Likes.Count(),
                        c.CreatedOn,
                        c.UserId,
                        c.PostId,
                        c.User.PicturePath,
                        c.Id
                       ))
               .FirstOrDefaultAsync();
        }
    }
}
