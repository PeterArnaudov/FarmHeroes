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

            builder.HasMany(i => i.Storage)
                .WithOne(e => e.Inventory)
                .HasForeignKey(e => e.InventoryId);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Inventory)
                .HasForeignKey<Hero>(h => h.InventoryId);
        }
    }
}
