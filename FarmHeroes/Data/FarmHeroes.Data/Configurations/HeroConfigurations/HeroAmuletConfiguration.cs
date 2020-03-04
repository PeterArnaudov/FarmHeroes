namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HeroAmuletConfiguration : IEntityTypeConfiguration<HeroAmulet>
    {
        public void Configure(EntityTypeBuilder<HeroAmulet> builder)
        {
            builder.ToTable("HeroAmulets");

            builder.HasKey(ha => ha.Id);

            builder.HasOne(ha => ha.EquippedSet)
                .WithOne(es => es.Amulet)
                .HasForeignKey<EquippedSet>(es => es.AmuletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ha => ha.Inventory)
                .WithMany(i => i.Amulets)
                .HasForeignKey(ha => ha.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ha => ha.InventoryId)
                .IsRequired();

            builder.Property(ha => ha.Name)
                .IsRequired();

            builder.Property(ha => ha.Description)
                .IsRequired();

            builder.Property(ha => ha.ImageUrl)
                .IsRequired();

            builder.Property(ha => ha.InitialBonus)
                .IsRequired();

            builder.Property(ha => ha.InitialPrice)
                .IsRequired();

            builder.Property(ha => ha.Bonus)
                .IsRequired();
        }
    }
}
