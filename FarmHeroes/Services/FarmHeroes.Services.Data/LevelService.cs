namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.DatabaseModels;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.LevelModels;
    using Microsoft.EntityFrameworkCore;

    public class LevelService : ILevelService
    {
        private const string LevelNotificationImageUrl = "/images/notifications/level-notification.png";
        private const string LevelNotificationTitle = "Level up";
        private const string LevelNotificationContent = "You just reached level {0}. Congratulations!";

        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;
        private readonly INotificationService notificationService;

        public LevelService(FarmHeroesDbContext context, IHeroService heroService, INotificationService notificationService)
        {
            this.context = context;
            this.heroService = heroService;
            this.notificationService = notificationService;
        }

        public async Task<int> GetCurrentHeroLevel()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            return hero.Level.CurrentLevel;
        }

        public async Task GiveHeroExperience(int experience, int id = 0)
        {
            Hero hero = id == 0 ? await this.heroService.GetCurrentHero() : await this.heroService.GetHeroById(id);

            if (hero.Level.CurrentLevel == 100)
            {
                return;
            }

            hero.Level.CurrentExperience += experience;

            if (hero.Level.CurrentExperience >= hero.Level.NeededExperience)
            {
                await this.LevelUpHero();
            }

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateLevel(LevelModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            DatabaseLevel databaseLevel = await this.context.DatabaseLevels.SingleAsync(l => l.Level == inputModel.LevelCurrentLevel);

            hero.Level.CurrentLevel = databaseLevel.Level;
            hero.Level.NeededExperience = databaseLevel.NeededExperience;
            hero.Level.CurrentExperience = 0;

            await this.context.SaveChangesAsync();
        }

        private async Task LevelUpHero(int id = 0)
        {
            Hero hero = id == 0 ? await this.heroService.GetCurrentHero() : await this.heroService.GetHeroById(id);

            if (hero.Level.CurrentLevel == 100)
            {
                return;
            }

            DatabaseLevel databaseLevel = await this.context.DatabaseLevels.SingleAsync(l => l.Level == hero.Level.CurrentLevel + 1);

            hero.Level.CurrentExperience = hero.Level.CurrentExperience - hero.Level.NeededExperience;
            hero.Level.CurrentLevel = databaseLevel.Level;
            hero.Level.NeededExperience = databaseLevel.NeededExperience;

            Notification notification = new Notification()
            {
                ImageUrl = LevelNotificationImageUrl,
                Title = LevelNotificationTitle,
                Content = string.Format(LevelNotificationContent, hero.Level.CurrentLevel),
                Type = NotificationType.Other,
                Hero = hero,
            };
            await this.notificationService.AddNotification(notification);

            await this.context.SaveChangesAsync();
        }
    }
}
