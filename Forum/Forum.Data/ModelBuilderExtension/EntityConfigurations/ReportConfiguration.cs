using Forum.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Data.ModelBuilderExtension.EntityConfigurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasOne(r => r.Sender)
                   .WithMany(u => u.SendReports)
                   .HasForeignKey(r => r.SenderId);

            builder.HasOne(r => r.Receiver)
                   .WithMany(u => u.ReceivedReports)
                   .HasForeignKey(r => r.ReceiverId);

            builder.HasOne(r => r.ReportType)
                   .WithMany(rt => rt.Reports)
                   .HasForeignKey(r => r.ReportTypeId);
        }
    }
}