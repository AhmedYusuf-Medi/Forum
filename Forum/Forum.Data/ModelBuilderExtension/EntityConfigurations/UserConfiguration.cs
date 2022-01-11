using Forum.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Username)
                   .IsUnique();

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasOne(u => u.Role)
                   .WithMany(r => r.Users)
                   .HasForeignKey(u => u.RoleId);

            builder.HasCheckConstraint("Constraint_Role",
                                       "RoleId = 1 or RoleId = 2 or RoleId = 3 or RoleId = 4");
        }
    }
}