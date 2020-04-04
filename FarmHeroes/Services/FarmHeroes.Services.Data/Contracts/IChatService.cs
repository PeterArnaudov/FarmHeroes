namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.Chat;
    using FarmHeroes.Web.ViewModels.ChatModels;
    using System;
    using System.Threading.Tasks;

    public interface IChatService
    {
        Task<ChatViewModel> GetAllMessagesToViewModel();

        Task AddMessage(Message message);
    }
}
