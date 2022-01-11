using Forum.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(u => u.UserId);

            builder.HasOne(p => p.Category)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(u => u.CategoryId);
        }
    }
}