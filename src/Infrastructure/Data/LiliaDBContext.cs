// src/Infrastructure/Data/LiliaDBContext.cs

using Microsoft.EntityFrameworkCore;
using Lilia.Domain.Entities;
using Lilia.Domain.Entities.Units;

namespace Lilia.Infrastructure.Data
{
    public class LiliaDBContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<BaseUnit> Units { get; set; }

        public LiliaDBContext(DbContextOptions<LiliaDBContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.DiscordID);
                entity.Property(p => p.DiscordID).ValueGeneratedNever();

                entity.OwnsOne(p => p.UnitCount, uc =>
                {
                    uc.ToJson();
                });

                entity.HasMany(p => p.Organisations)
                      .WithOne(o => o.Player)
                      .HasForeignKey(o => o.PlayerDiscordID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.HasKey(o => o.InternalID);
                entity.HasIndex(o => new { o.PlayerDiscordID, o.ShortCode }).IsUnique();

                entity.HasMany(o => o.SubOrganisations)
                      .WithOne(o => o.ParentOrganisation)
                      .HasForeignKey(o => o.ParentOrganisationID)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasMany(o => o.AssignedUnits)
                      .WithOne(u => u.Organisation)
                      .HasForeignKey(u => u.OrganisationID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BaseUnit>(entity =>
            {
                entity.HasKey(u => u.InternalID);

                entity.HasDiscriminator<string>("UnitType")
                      .HasValue<Starfighter>("Starfighter");
            });
        }
    }
}