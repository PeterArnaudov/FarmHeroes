namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class HeroBonusConfigurations : IEntityTypeConfiguration<HeroBonus>
    {
        public void Configure(EntityTypeBuilder<HeroBonus> builder)
        {
            builder.ToTable("HeroBonuses");

            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Hero)
                .WithMany(h => h.Bonuses)
                .HasForeignKey(b => b.HeroId);
        }
    }
}
