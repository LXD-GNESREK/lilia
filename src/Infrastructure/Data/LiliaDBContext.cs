// src/Infrastructure/Data/LiliaDBContext.cs

using Microsoft.EntityFrameworkCore;
using Lilia.Domain.Entities;

namespace Lilia.Infrastructure.Data
{
    public class LiliaDBContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

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

                entity.Ignore(p => p.Organisations);
            });
        }
    }
}