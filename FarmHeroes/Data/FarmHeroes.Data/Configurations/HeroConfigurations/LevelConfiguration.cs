namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.ToTable("Levels");

            builder.HasKey(l => l.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Level)
                .HasForeignKey<Hero>(h => h.LevelId);
        }
    }
}
