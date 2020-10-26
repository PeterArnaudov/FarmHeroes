namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using FarmHeroes.Data.Models.HeroModels;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PremiumFeaturesConfiguration : IEntityTypeConfiguration<PremiumFeatures>
    {
        public void Configure(EntityTypeBuilder<PremiumFeatures> builder)
        {
            builder.ToTable("PremiumFeatures");

            builder.HasKey(pf => pf.Id);

            builder.HasOne(pf => pf.Hero)
                .WithOne(h => h.PremiumFeatures)
                .HasForeignKey<Hero>(h => h.PremiumFeaturesId);
        }
    }
}
