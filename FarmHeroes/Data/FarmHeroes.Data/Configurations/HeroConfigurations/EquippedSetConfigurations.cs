namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EquippedSetConfigurations : IEntityTypeConfiguration<EquippedSet>
    {
        public void Configure(EntityTypeBuilder<EquippedSet> builder)
        {
            builder.ToTable("EquippedSets");

            builder.HasKey(es => es.Id);

            builder.HasMany(es => es.Equipped)
                .WithOne(he => he.EquippedSet)
                .HasForeignKey(he => he.EquippedSetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
