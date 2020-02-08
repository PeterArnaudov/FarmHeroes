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

            builder.HasOne(es => es.Helmet)
                .WithOne(h => h.EquippedSet)
                .HasForeignKey<EquippedSet>(es => es.HelmetId);

            builder.HasOne(es => es.Armor)
                .WithOne(a => a.EquippedSet)
                .HasForeignKey<EquippedSet>(es => es.ArmorId);

            builder.HasOne(es => es.Weapon)
                .WithOne(w => w.EquippedSet)
                .HasForeignKey<EquippedSet>(es => es.WeaponId);

            builder.HasOne(es => es.Shield)
                .WithOne(s => s.EquippedSet)
                .HasForeignKey<EquippedSet>(es => es.ShieldId);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.EquippedSet)
                .HasForeignKey<Hero>(h => h.EquippedSetId);

            builder.Property(es => es.HelmetId)
                .IsRequired(false);

            builder.Property(es => es.ArmorId)
                .IsRequired(false);

            builder.Property(es => es.WeaponId)
                .IsRequired(false);

            builder.Property(es => es.ShieldId)
                .IsRequired(false);
        }
    }
}
