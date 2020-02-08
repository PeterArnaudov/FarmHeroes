namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StatisticsConfiguration : IEntityTypeConfiguration<Statistics>
    {
        public void Configure(EntityTypeBuilder<Statistics> builder)
        {
            builder.ToTable("Statistics");

            builder.HasKey(s => s.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Statistics)
                .HasForeignKey<Hero>(h => h.StatisticsId);
        }
    }
}
