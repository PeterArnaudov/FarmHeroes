namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CharacteristicsConfiguration : IEntityTypeConfiguration<Characteristics>
    {
        public void Configure(EntityTypeBuilder<Characteristics> builder)
        {
            builder.ToTable("Characteristics");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Characteristics)
                .HasForeignKey<Hero>(h => h.CharacteristicsId);
        }
    }
}
