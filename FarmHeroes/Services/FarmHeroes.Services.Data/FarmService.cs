namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class FarmService : IFarmService
    {
        private const int WorkDurationInSeconds = 14400;
        private const int WorkDurationInHours = WorkDurationInSeconds / 3600;
        private const string FarmNotificationImageUrl = "/images/notifications/farm-notification.png";
        private const string FarmNotificationTitle = "Farm report";
        private const string FarmNotificationContent = "You finished your work on the farm.";

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly ILevelService levelService;
        private readonly IChronometerService chronometerService;
        private readonly INotificationService notificationService;
        private readonly IAmuletBagService amuletBagService;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor context;

        public FarmService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, ILevelService levelService, IChronometerService chronometerService, INotificationService notificationService, IAmuletBagService amuletBagService, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor context)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.levelService = levelService;
            this.chronometerService = chronometerService;
            this.notificationService = notificationService;
            this.amuletBagService = amuletBagService;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.context = context;
        }

        public async Task<int> StartWork()
        {
            await this.amuletBagService.EquipAmulet("Farm");

            await this.chronometerService.SetWorkUntil(WorkDurationInSeconds, WorkStatus.Farm);

            return WorkDurationInSeconds;
        }

        public async Task<CollectedResourcesViewModel> Collect()
        {
            CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();
            Hero hero = await this.heroService.GetHero();

            this.CheckIfHeroWorkedOnFarm(hero);

            HeroAmulet heroAmulet = hero.EquippedSet.Amulet;
            collectedResources.Experience = FarmFormulas.CalculateExperience(hero.Level.CurrentLevel, WorkDurationInHours);
            collectedResources.Gold = FarmFormulas.CalculateGoldEarned(hero.Level.CurrentLevel, WorkDurationInHours, heroAmulet?.Name == AmuletNames.Laborium ? heroAmulet.Bonus : 0);

            hero.Statistics.EarnedOnFarm += collectedResources.Gold;

            await this.levelService.GiveHeroExperience(collectedResources.Experience);
            await this.resourcePouchService.IncreaseResource(ResourceNames.Gold, collectedResources.Gold);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            Notification notification = new Notification()
            {
                ImageUrl = FarmNotificationImageUrl,
                Title = FarmNotificationTitle,
                Content = FarmNotificationContent,
                Gold = collectedResources.Gold,
                Experience = collectedResources.Experience,
                Type = NotificationType.Farm,
                Hero = hero,
            };
            await this.notificationService.AddNotification(notification);

            return collectedResources;
        }

        private void CheckIfHeroWorkedOnFarm(Hero hero)
        {
            if (hero.WorkStatus != WorkStatus.Farm || hero.Chronometer.WorkUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    FarmExceptionMessages.CannotCollectRewardMessage,
                    FarmExceptionMessages.CannotCollectRewardInstruction,
                    Redirects.FarmRedirect,
                    true);
            }
        }
    }
}
