using AdvSearcher.Core.Entities.AdvertisementAttachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Sqlite.Metadata.Internal;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class AttachmentsRepositoryConfiguration
    : IEntityTypeConfiguration<AdvertisementAttachment>
{
    public void Configure(EntityTypeBuilder<AdvertisementAttachment> builder)
    {
        builder.HasKey(at => at.Id).HasAnnotation(SqliteAnnotationNames.Autoincrement, true);
        builder
            .Property(at => at.Id)
            .HasConversion(id => id.Value, value => AdvertisementAttachmendId.Create(value));
        builder.ComplexProperty(
            att => att.Url,
            cpb =>
            {
                cpb.Property(val => val.Value);
            }
        );
    }
}
