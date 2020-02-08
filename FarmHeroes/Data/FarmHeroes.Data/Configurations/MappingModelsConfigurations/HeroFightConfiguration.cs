namespace FarmHeroes.Data.Configurations.MappingModelsConfigurations
{
    using FarmHeroes.Data.Models.MappingModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HeroFightConfiguration : IEntityTypeConfiguration<HeroFight>
    {
        public void Configure(EntityTypeBuilder<HeroFight> builder)
        {
            builder.HasKey(x => new { x.HeroId, x.FightId });

            builder.ToTable("HeroFights");

            builder.HasOne(x => x.Hero)
                .WithMany(h => h.HeroFights)
                .HasForeignKey(x => x.HeroId);

            builder.HasOne(x => x.Fight)
                .WithMany(f => f.HeroFights)
                .HasForeignKey(x => x.FightId);
        }
    }
}
