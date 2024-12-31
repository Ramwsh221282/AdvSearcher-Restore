using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class AdvertisementsRepositoryConfiguration
    : IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.HasKey(ad => ad.Id);
        builder
            .Property(ad => ad.Id)
            .HasConversion(id => id.Id, value => AdvertisementId.Create(value));
        builder.HasOne(ad => ad.Publisher);
        builder
            .HasMany(ad => ad.Attachments)
            .WithOne(att => att.Advertisement)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .ComplexProperty(
                ad => ad.Content,
                cpb =>
                {
                    cpb.Property(val => val.Content);
                }
            )
            .HasIndex()
            .IsUnique();
        builder.ComplexProperty(
            ad => ad.Date,
            cpb =>
            {
                cpb.Property(val => val.Value);
            }
        );
        builder.ComplexProperty(
            ad => ad.Url,
            cpb =>
            {
                cpb.Property(val => val.Url);
            }
        );
        builder.ComplexProperty(
            ad => ad.ServiceName,
            cpb =>
            {
                cpb.Property(val => val.ServiceName);
            }
        );
        builder.ComplexProperty(
            ad => ad.CreationDate,
            cpb =>
            {
                cpb.Property(val => val.Date);
            }
        );
    }
}
