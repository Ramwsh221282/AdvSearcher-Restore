using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.AdvertisementAttachments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Sqlite.Metadata.Internal;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class AttachmentsRepositoryConfiguration
    : IEntityTypeConfiguration<AdvertisementAttachment>
{
    public void Configure(EntityTypeBuilder<AdvertisementAttachment> builder)
    {
        builder.HasKey(at => at.Id);
        builder
            .Property(at => at.Id)
            .HasConversion(id => id.Value, value => AdvertisementAttachmentId.Create(value))
            .HasAnnotation(SqliteAnnotationNames.Autoincrement, true)
            .ValueGeneratedOnAdd();

        builder.ComplexProperty(
            att => att.Url,
            cpb =>
            {
                cpb.Property(val => val.Value);
            }
        );
    }
}
