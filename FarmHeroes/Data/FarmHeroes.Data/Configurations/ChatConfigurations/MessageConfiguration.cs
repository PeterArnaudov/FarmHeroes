namespace FarmHeroes.Data.Configurations.ChatConfigurations
{
    using FarmHeroes.Data.Models.Chat;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.ToTable("Messages");
        }
    }
}
