//Local
using Forum.Data;
using Forum.Models.Response;
using Forum.Service.Common.Exceptions;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Threading.Tasks;
//Static
using static Forum.Service.Common.Message.Message;

namespace Forum.Service.Common.Extensions
{
    public static class Validator
    {
        public static void CheckIsOwner(long userId, long? currentUserId, InfoResponse response)
        {
            if (userId != currentUserId)
            {
                response.IsSuccess = false;
                response.Message = ExceptionMessages.Invalid_Credentials;
            }
        }

        public static void ValidateForNull<T>(Response<T> responseModel, string succeedMessage, string entityType)
        {
            if (responseModel.Payload == null)
            {
                responseModel.Message = string.Format(ExceptionMessages.DOESNT_EXIST, entityType);
            }
            else
            {
                responseModel.Message = string.Format(succeedMessage, entityType);
                responseModel.IsSuccess = true;
            }
        }

        public static void ValidateForNull<T>(T entity, InfoResponse responseModel, string succeedMessage
                                             ,string entityType)
        {
            if (entity == null)
            {
                responseModel.Message = string.Format(ExceptionMessages.DOESNT_EXIST, entityType); 
            }
            else
            {
                responseModel.Message = string.Format(succeedMessage, entityType);
                responseModel.IsSuccess = true;
            }
        }

        internal static class DataValidations
        {
            public static bool IsStringPropertyValid(string property, string currentProperty)
            {
                return !string.IsNullOrEmpty(property)
                    && !string.IsNullOrWhiteSpace(property)
                    && !currentProperty.Equals(property);
            }
        }

        internal static class CriteriaValidations 
        {
            public static bool BySingleNullableId(long? userId)
            {
                return userId.HasValue;
            }

            public static bool BySingleCriteria(string bySingleCriteria)
            {
                return !string.IsNullOrEmpty(bySingleCriteria);
            }
        }

        internal static class ForeignKeyValidations
        {
            public static async Task CheckUser(long userId, ForumDbContext db, string message)
            {
                if (!await db.Users.AnyAsync(u => u.Id == userId))
                {
                    throw new BadRequestException(message);
                }
            }

            public static async Task CheckPost(long postId, ForumDbContext db, string message)
            {
                if (!await db.Posts.AnyAsync(u => u.Id == postId))
                {
                    throw new BadRequestException(message);
                }
            }

            public static async Task CheckComment(long commentId, ForumDbContext db, string message)
            {
                if (!await db.Comments.AnyAsync(u => u.Id == commentId))
                {
                    throw new BadRequestException(message);
                }
            }

            public static void CheckReport(bool isFirstReport, string entityType, InfoResponse responseModel)
            {
                if (isFirstReport)
                {
                    responseModel.Message = string.Format(ExceptionMessages.Already_Reported, entityType);
                }
                else
                {
                    responseModel.IsSuccess = true;
                }
            }

            public static async Task CheckCategory(long categoryId, ForumDbContext db, string message)
            {
                if (!await db.Categories.AnyAsync(c => c.Id == categoryId))
                {
                    throw new BadRequestException(message);
                }
            }

            public static async Task CheckReportType(long reportTypeId, ForumDbContext db, string message)
            {
                if (!await db.ReportTypes.AnyAsync(c => c.Id == reportTypeId))
                {
                    throw new BadRequestException(message);
                }
            }

            public static bool CheckForExistingRealitons(long count, InfoResponse response, string message)
            {
                if (count > 0)
                {
                    response.Message = message;
                    return true;
                }

                return false;
            }
        }
    }
}