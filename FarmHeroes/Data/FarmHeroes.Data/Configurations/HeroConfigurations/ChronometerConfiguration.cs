namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ChronometerConfiguration : IEntityTypeConfiguration<Chronometer>
    {
        public void Configure(EntityTypeBuilder<Chronometer> builder)
        {
            builder.ToTable("Chronometers");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Chronometer)
                .HasForeignKey<Hero>(h => h.ChronometerId);
        }
    }
}
