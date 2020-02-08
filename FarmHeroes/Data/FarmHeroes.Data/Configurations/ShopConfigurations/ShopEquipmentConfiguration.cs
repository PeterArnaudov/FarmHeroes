namespace FarmHeroes.Data.Configurations.ShopConfigurations
{
    using System;

    using FarmHeroes.Data.Models.ShopModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShopEquipmentConfiguration : IEntityTypeConfiguration<ShopEquipment>
    {
        public void Configure(EntityTypeBuilder<ShopEquipment> builder)
        {
            builder.ToTable("ShopEquipments");

            builder.HasKey(se => se.Id);

            builder.Property(se => se.Name)
                .IsRequired();

            builder.Property(se => se.ImageUrl)
                .IsRequired();
        }
    }
}
