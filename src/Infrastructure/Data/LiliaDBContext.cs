// src/Infrastructure/Data/LiliaDBContext.cs

using Microsoft.EntityFrameworkCore;
using Lilia.Domain.Entities;
using Lilia.Domain.Entities.Units;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Lilia.Infrastructure.Data
{
    public class LiliaDBContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<BaseUnit> Units { get; set; }
        public DbSet<RegistryUnit> RegistryUnits { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopRegistryUnit> ShopRegistryUnits { get; set; }

        public LiliaDBContext(DbContextOptions<LiliaDBContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dictionaryComparer = new ValueComparer<Dictionary<string, int>>
            (
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            );

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.DiscordID);
                entity.Property(p => p.DiscordID).ValueGeneratedNever();

                entity.OwnsOne(p => p.UnitCount, uc =>
                {
                    uc.Property(u => u.Units)
                      .HasConversion
                      (
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<Dictionary<string, int>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, int>()
                      )
                      .Metadata.SetValueComparer(dictionaryComparer);
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

            modelBuilder.Entity<ShopRegistryUnit>().HasKey(sru => new { sru.ShopID, sru.RegistryUnitID });

            modelBuilder.Entity<ShopRegistryUnit>().HasOne(sru => sru.Shop)
                                                   .WithMany(s => s.AvailableUnits)
                                                   .HasForeignKey(sru => sru.ShopID);
            
            modelBuilder.Entity<ShopRegistryUnit>().HasOne(sru => sru.RegistryUnit)
                                                   .WithMany()
                                                   .HasForeignKey(sru => sru.RegistryUnitID);
            
            modelBuilder.Entity<BaseUnit>().HasOne(u => u.Blueprint)
                                           .WithMany()
                                           .HasForeignKey(u => u.RegistryUnitID)
                                           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}