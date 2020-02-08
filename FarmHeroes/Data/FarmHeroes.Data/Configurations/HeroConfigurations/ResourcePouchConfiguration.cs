namespace FarmHeroes.Data.Configurations.HeroConfigurations
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ResourcePouchConfiguration : IEntityTypeConfiguration<ResourcePouch>
    {
        public void Configure(EntityTypeBuilder<ResourcePouch> builder)
        {
            builder.ToTable("ResourcePouches");

            builder.HasKey(r => r.Id);

            builder.HasOne(c => c.Hero)
                .WithOne(h => h.ResourcePouch)
                .HasForeignKey<Hero>(h => h.ResourcePouchId);
        }
    }
}
