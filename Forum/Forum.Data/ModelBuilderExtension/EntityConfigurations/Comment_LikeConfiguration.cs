using Forum.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class Comment_LikeConfiguration : IEntityTypeConfiguration<Comment_Like>
    {
        public void Configure(EntityTypeBuilder<Comment_Like> builder)
        {
            builder.HasOne(c => c.User)
                   .WithMany(u => u.CommentLikes)
                   .HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.Comment)
                   .WithMany(u => u.Likes)
                   .HasForeignKey(c => c.CommentId);
        }
    }
}