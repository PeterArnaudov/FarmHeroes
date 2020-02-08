namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HealthConfiguration : IEntityTypeConfiguration<Health>
    {
        public void Configure(EntityTypeBuilder<Health> builder)
        {
            builder.ToTable("Healths");

            builder.HasKey(h => h.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.Health)
                .HasForeignKey<Hero>(h => h.HealthId);
        }
    }
}
