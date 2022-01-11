using Forum.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class CommentReportConfiguration : IEntityTypeConfiguration<CommentReport>
    {
        public void Configure(EntityTypeBuilder<CommentReport> builder)
        {
            builder.HasOne(r => r.Report)
                   .WithMany(u => u.CommentReports)
                   .HasForeignKey(r => r.ReportId);

            builder.HasOne(r => r.Comment)
                   .WithMany(u => u.Reports)
                   .HasForeignKey(r => r.CommentId);

            builder.HasKey(p => new { p.ReportId, p.CommentId });
        }
    }
}