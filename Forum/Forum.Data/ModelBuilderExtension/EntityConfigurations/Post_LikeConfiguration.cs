using Forum.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class Post_LikeConfiguration : IEntityTypeConfiguration<Post_Like>
    {
        public void Configure(EntityTypeBuilder<Post_Like> builder)
        {
            builder.HasOne(c => c.User)
                   .WithMany(u => u.PostLikes)
                   .HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.Post)
                   .WithMany(u => u.Likes)
                   .HasForeignKey(c => c.PostId);
        }
    }
}