using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Persistance.SQLite.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AdvSearcher.Persistance.SQLite;

public sealed class AppDbContext : DbContext
{
    public DbSet<Advertisement> Advertisements { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<AdvertisementAttachment> Attachments { get; set; } = null!;
    public DbSet<ServiceUrl> ServiceUrls { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder) =>
        contextOptionsBuilder.UseSqlite("Data Source=AdvSearcher.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder
            .ApplyConfiguration(new AdvertisementsRepositoryConfiguration())
            .ApplyConfiguration(new PublishersRepositoryConfiguration())
            .ApplyConfiguration(new AttachmentsRepositoryConfiguration())
            .ApplyConfiguration(new ServiceUrlRepositoryConfiguration());
}
