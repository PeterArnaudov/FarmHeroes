namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ChatModels;
    using FarmHeroes.Web.ViewModels.NotificationModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            ChatViewModel viewModel = await this.chatService.GetAllMessagesToViewModel();
            return this.View(viewModel);
        }
    }
}
