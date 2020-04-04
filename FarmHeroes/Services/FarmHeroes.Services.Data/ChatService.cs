namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Chat;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ChatModels;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class ChatService : IChatService
    {
        private readonly FarmHeroesDbContext context;

        public ChatService(FarmHeroesDbContext context)
        {
            this.context = context;
        }

        public async Task<ChatViewModel> GetAllMessagesToViewModel()
        {
            Message[] messages = await this.context
                .Messages
                .ToArrayAsync();

            messages = messages
                .OrderByDescending(x => DateTime.ParseExact(x.CreatedOn, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture))
                .ToArray();

            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Messages = messages,
            };

            return chatViewModel;
        }

        public async Task AddMessage(Message message)
        {
            if (message.Text.Length < 1)
            {
                throw new Exception("The message has to contain text.");
            }

            await this.context.Messages.AddAsync(message);
            await this.context.SaveChangesAsync();
        }
    }
}
