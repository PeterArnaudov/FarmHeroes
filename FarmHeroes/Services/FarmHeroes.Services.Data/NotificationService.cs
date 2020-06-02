namespace FarmHeroes.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class NotificationService : INotificationService
    {
        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public NotificationService(IHeroService heroService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.heroService = heroService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TViewModel> GetAllNotifications<TViewModel>()
        {
            int heroId = this.heroService.GetHero().Result.Id;

            Notification[] notifications = this.context.Notifications
                .Where(n => n.HeroId == heroId)
                .OrderByDescending(n => n.CreatedOn)
                .ToArray();

            TViewModel viewModel = this.mapper.Map<TViewModel>(notifications);

            await this.MarkAsRead();

            return viewModel;
        }

        public async Task AddNotification(Notification notification)
        {
            await this.context.Notifications.AddAsync(notification);

            await this.context.SaveChangesAsync();
        }

        public async Task<int> GetNewNotificationsCount()
        {
            int heroId = this.heroService.GetHero().Result.Id;

            int count = await this.context.Notifications
                .Where(n => n.HeroId == heroId && n.IsNew)
                .CountAsync();

            return count;
        }

        public async Task DeleteOld()
        {
            Notification[] oldNotifications = await this.context.Notifications
                .Where(n => n.CreatedOn <= DateTime.UtcNow.AddDays(-1))
                .ToArrayAsync();

            this.context.Notifications.RemoveRange(oldNotifications);

            await this.context.SaveChangesAsync();
        }

        private async Task MarkAsRead()
        {
            int heroId = this.heroService.GetHero().Result.Id;

            List<Notification> newNotifications = await this.context.Notifications
                .Where(n => n.HeroId == heroId && n.IsNew)
                .ToListAsync();

            newNotifications.ForEach(n => n.IsNew = false);

            await this.context.SaveChangesAsync();
        }
    }
}
