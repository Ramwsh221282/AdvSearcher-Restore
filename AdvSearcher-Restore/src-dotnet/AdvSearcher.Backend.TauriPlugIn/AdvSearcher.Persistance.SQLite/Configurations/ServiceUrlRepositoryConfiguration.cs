using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Sqlite.Metadata.Internal;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class ServiceUrlRepositoryConfiguration : IEntityTypeConfiguration<ServiceUrl>
{
    public void Configure(EntityTypeBuilder<ServiceUrl> builder)
    {
        builder.HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasConversion(id => id.Value, value => ServiceUrlId.Create(value))
            .ValueGeneratedOnAdd()
            .HasAnnotation(SqliteAnnotationNames.Autoincrement, true);

        builder.ComplexProperty(
            s => s.Mode,
            cpb =>
            {
                cpb.Property(val => val.Mode);
            }
        );
        builder.ComplexProperty(
            s => s.Service,
            cpb =>
            {
                cpb.Property(val => val.Name);
            }
        );
        builder.ComplexProperty(
            s => s.Value,
            cpb =>
            {
                cpb.Property(url => url.Value);
            }
        );
    }
}
