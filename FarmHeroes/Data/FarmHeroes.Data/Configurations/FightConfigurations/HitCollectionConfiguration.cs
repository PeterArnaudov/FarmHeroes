namespace FarmHeroes.Data.Configurations.FightConfigurations
{
    using FarmHeroes.Data.Models.FightModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HitCollectionConfiguration : IEntityTypeConfiguration<HitCollection>
    {
        public void Configure(EntityTypeBuilder<HitCollection> builder)
        {
            builder.ToTable("HitCollections");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Fight)
                .WithOne(f => f.HitCollection)
                .HasForeignKey<Fight>(f => f.HitCollectionId);
        }
    }
}
