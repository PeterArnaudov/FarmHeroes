namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DailyLimitsConfiguration : IEntityTypeConfiguration<DailyLimits>
    {
        public void Configure(EntityTypeBuilder<DailyLimits> builder)
        {
            builder.ToTable("DailyLimits");

            builder.HasKey(h => h.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.DailyLimits)
                .HasForeignKey<Hero>(h => h.DailyLimitsId);
        }
    }
}
