namespace FarmHeroes.Web.Components
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Microsoft.AspNetCore.Mvc;

    public class NewNotificationsCountViewComponent : ViewComponent
    {
        private readonly INotificationService notificationService;

        public NewNotificationsCountViewComponent(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = await this.notificationService.GetNewNotificationsCount();

            return this.View(count);
        }
    }
}
