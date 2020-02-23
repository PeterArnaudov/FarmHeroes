namespace FarmHeroes.Data.Configurations.NotificationConfigurations
{
    using System;

    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.Hero)
                .WithMany(h => h.Notifications)
                .HasForeignKey(n => n.HeroId);
        }
    }
}
