using AdvSearcher.Core.Entities.Publishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Sqlite.Metadata.Internal;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class PublishersRepositoryConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(p => p.Id).HasAnnotation(SqliteAnnotationNames.Autoincrement, true);
        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PublisherId.Create(value));
        builder
            .ComplexProperty(
                pub => pub.Data,
                cpb =>
                {
                    cpb.Property(val => val.Value);
                }
            )
            .HasIndex()
            .IsUnique();
    }
}
