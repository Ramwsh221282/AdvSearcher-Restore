using AdvSearcher.Core.Entities.Publishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Sqlite.Metadata.Internal;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class PublishersRepositoryConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PublisherId.Create(value))
            .HasAnnotation(SqliteAnnotationNames.Autoincrement, true)
            .ValueGeneratedOnAdd();

        builder.ComplexProperty(
            pub => pub.Data,
            cpb =>
            {
                cpb.Property(val => val.Value);
            }
        );
        builder.Property(pub => pub.IsIgnored);
    }
}
