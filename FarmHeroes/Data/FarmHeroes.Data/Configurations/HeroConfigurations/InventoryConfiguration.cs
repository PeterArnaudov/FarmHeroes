namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventories");

            builder.HasKey(i => i.Id);

            builder.HasMany(i => i.Items)
                .WithOne(e => e.Inventory)
                .HasForeignKey(e => e.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.Amulets)
                .WithOne(a => a.Inventory)
                .HasForeignKey(a => a.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Inventory)
                .HasForeignKey<Hero>(h => h.InventoryId);
        }
    }
}
