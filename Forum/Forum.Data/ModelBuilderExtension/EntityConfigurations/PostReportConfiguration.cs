using Forum.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class PostReportConfiguration : IEntityTypeConfiguration<PostReport>
    {
        public void Configure(EntityTypeBuilder<PostReport> builder)
        {
            builder.HasOne(r => r.Report)
                   .WithMany(u => u.PostReports)
                   .HasForeignKey(r => r.ReportId);

            builder.HasOne(r => r.Post)
                   .WithMany(u => u.Reports)
                   .HasForeignKey(r => r.PostId);

            builder.HasKey(p => new { p.ReportId, p.PostId });
        }
    }
}