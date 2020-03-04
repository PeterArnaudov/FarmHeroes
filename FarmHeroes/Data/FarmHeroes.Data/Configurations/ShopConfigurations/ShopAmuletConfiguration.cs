namespace FarmHeroes.Data.Configurations.ShopConfigurations
{
    using FarmHeroes.Data.Models.ShopModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShopAmuletConfiguration : IEntityTypeConfiguration<ShopAmulet>
    {
        public void Configure(EntityTypeBuilder<ShopAmulet> builder)
        {
            builder.ToTable("ShopAmulets");

            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.Name)
                .IsRequired();

            builder.Property(sa => sa.Description)
                .IsRequired();

            builder.Property(sa => sa.ImageUrl)
                .IsRequired();

            builder.Property(sa => sa.InitialBonus)
                .IsRequired();

            builder.Property(sa => sa.InitialPrice)
                .IsRequired();
        }
    }
}
