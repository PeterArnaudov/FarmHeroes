namespace FarmHeroes.Data.Configurations.ShopConfigurations
{
    using FarmHeroes.Data.Models.ShopModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class ShopBonusConfigurations : IEntityTypeConfiguration<ShopBonus>
    {
        public void Configure(EntityTypeBuilder<ShopBonus> builder)
        {
            builder.ToTable("ShopBonuses");

            builder.HasKey(b => b.Id);
        }
    }
}
