namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AmuletBagConfiguration : IEntityTypeConfiguration<AmuletBag>
    {
        public void Configure(EntityTypeBuilder<AmuletBag> builder)
        {
            builder.ToTable("AmuletBags");

            builder.HasKey(i => i.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.AmuletBag)
                .HasForeignKey<Hero>(h => h.AmuletBagId);
        }
    }
}
