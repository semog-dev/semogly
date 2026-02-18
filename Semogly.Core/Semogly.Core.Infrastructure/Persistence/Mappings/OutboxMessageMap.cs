using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Semogly.Core.Infrastructure.Outbox;

namespace Semogly.Core.Infrastructure.Persistence.Mappings;

public class OutboxMessageMap : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_message");
        builder.HasKey(x => x.Id).HasName("pk_outbox_message");
        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(x => x.OccurredOnUtc)
            .HasColumnName("occurred_on")
            .IsRequired();

        builder.Property(x => x.ProcessedOnUtc)
            .HasColumnName("processed_on")
            .IsRequired(false);

        builder.Property(x => x.Error)
            .HasColumnName("error")
            .IsRequired(false);
    }
}
