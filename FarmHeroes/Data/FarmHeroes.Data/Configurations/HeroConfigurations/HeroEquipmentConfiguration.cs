namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HeroEquipmentConfiguration : IEntityTypeConfiguration<HeroEquipment>
    {
        public void Configure(EntityTypeBuilder<HeroEquipment> builder)
        {
            builder.ToTable("HeroEquipments");

            builder.HasKey(he => he.Id);

            builder.HasOne(he => he.Inventory)
                .WithMany(i => i.Items)
                .HasForeignKey(he => he.InventoryId);

            builder.Property(he => he.InventoryId)
                .IsRequired();

            builder.Property(he => he.Name)
                .IsRequired();

            builder.Property(he => he.ImageUrl)
                .IsRequired();
        }
    }
}
