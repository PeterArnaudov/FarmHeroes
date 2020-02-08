namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.ToTable("Heroes");

            builder.HasKey(h => h.Id);

            builder.HasMany(h => h.Notifications)
                .WithOne(n => n.Hero)
                .HasForeignKey(n => n.HeroId);

            builder.HasMany(h => h.HeroFights)
                .WithOne(hf => hf.Hero)
                .HasForeignKey(hf => hf.HeroId);
        }
    }
}
