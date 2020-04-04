namespace FarmHeroes.Web.Hubs
{
    using FarmHeroes.Data.Models.Chat;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;

        public ChatHub(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task Send(string messageInput)
        {
            Message message = new Message()
            {
                Sender = this.Context.User.Identity.Name,
                Text = messageInput,
                CreatedOn = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss"),
            };

            await this.chatService.AddMessage(message);

            await this.Clients.All.SendAsync(
                "NewMessage",
                message);
        }

        public async override Task OnConnectedAsync()
        {
            Message message = new Message()
            {
                Sender = "System",
                Text = $"{this.Context.User.Identity.Name} has entered the chat.",
                CreatedOn = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss"),
            };

            await this.Clients.All.SendAsync(
                "NewSystemMessage",
                message);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            Message message = new Message()
            {
                Sender = "System",
                Text = $"{this.Context.User.Identity.Name} has left the chat.",
                CreatedOn = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss"),
            };

            await this.Clients.All.SendAsync(
                "NewSystemMessage",
                message);
        }
    }
}
