namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class FarmService : IFarmService
    {
        private const int WorkDurationInMinutes = 240;
        private const int WorkDurationInHours = WorkDurationInMinutes / 60;
        private const string FarmNotificationImageUrl = "/images/notifications/farm-notification.png";

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly ILevelService levelService;
        private readonly IChronometerService chronometerService;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor context;
        private readonly INotificationService notificationService;

        public FarmService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, ILevelService levelService, IChronometerService chronometerService, INotificationService notificationService, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor context)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.levelService = levelService;
            this.chronometerService = chronometerService;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.context = context;
            this.notificationService = notificationService;
        }

        public async Task<int> StartWork()
        {
            await this.chronometerService.SetWorkUntil(WorkDurationInMinutes, WorkStatus.Farm);

            return WorkDurationInHours;
        }

        public async Task<CollectedResourcesViewModel> Collect()
        {
            CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Farm)
            {
                throw new FarmHeroesException(
                    "You haven't been working on the farm or are still working there.",
                    "You have to cancel or finish your work before trying to collect.",
                    "/Farm");
            }

            HeroAmulet heroAmulet = hero.EquippedSet.Amulet;
            collectedResources.Experience = FarmFormulas.CalculateExperience(hero.Level.CurrentLevel, WorkDurationInHours);
            collectedResources.Gold = FarmFormulas.CalculateGoldEarned(hero.Level.CurrentLevel, WorkDurationInHours, heroAmulet.Name == "Laborium" ? heroAmulet.Bonus : 0);

            hero.Statistics.EarnedOnFarm += collectedResources.Gold;

            await this.levelService.GiveCurrentHeroExperience(collectedResources.Experience);
            await this.resourcePouchService.IncreaseCurrentHeroGold(collectedResources.Gold);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            Notification notification = new Notification()
            {
                ImageUrl = FarmNotificationImageUrl,
                Title = "Farm report",
                Content = $"You finished your work on the farm.",
                Gold = collectedResources.Gold,
                Experience = collectedResources.Experience,
                Type = NotificationType.Farm,
                Hero = hero,
            };
            await this.notificationService.AddNotification(notification);

            return collectedResources;
        }
    }
}
