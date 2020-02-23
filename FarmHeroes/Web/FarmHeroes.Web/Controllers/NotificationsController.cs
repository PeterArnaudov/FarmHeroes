﻿namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.NotificationModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class NotificationsController : BaseController
    {
        private readonly INotificationService notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task<IActionResult> All()
        {
            NotificationViewModel[] viewModel = await this.notificationService.GetAllNotifications<NotificationViewModel[]>();

            return this.View(viewModel);
        }
    }
}
