namespace FarmHeroes.Data.Configurations.FightConfigurations
{
    using FarmHeroes.Data.Models.FightModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FightConfiguration : IEntityTypeConfiguration<Fight>
    {
        public void Configure(EntityTypeBuilder<Fight> builder)
        {
            builder.HasKey(f => f.Id);

            builder.ToTable("Fights");

            builder.HasMany(f => f.HeroFights)
                .WithOne(hf => hf.Fight)
                .HasForeignKey(hf => hf.FightId);
        }
    }
}
