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
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class FarmService : IFarmService
    {
        private const int WorkDurationInMinutes = 240;
        private const int WorkDurationInHours = WorkDurationInMinutes / 60;
        private const string FarmNotificationImageUrl = "https://i.ibb.co/NrVTpR3/farm-notifications.png";

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

        public async Task StartWork()
        {
            Chronometer chronometer = await this.chronometerService.GetCurrentHeroChronometer();

            await this.chronometerService.SetWorkUntil(WorkDurationInMinutes, WorkStatus.Farm);
        }

        public async Task<int> Collect()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Farm)
            {
                throw new FarmHeroesException(
                    "You haven't been working on the farm or are still working there.",
                    "You have to cancel or finish your work before trying to collect.",
                    "/Farm");
            }

            int experience = FarmFormulas.CalculateExperience(hero.Level.CurrentLevel, WorkDurationInHours);
            int collected = FarmFormulas.CalculateGoldEarned(hero.Level.CurrentLevel, WorkDurationInHours);

            hero.Statistics.EarnedOnFarm += collected;

            await this.levelService.GiveCurrentHeroExperience(experience);
            await this.resourcePouchService.IncreaseCurrentHeroGold(collected);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            this.tempDataDictionaryFactory
                .GetTempData(this.context.HttpContext)
                .Add("Collected", $"You earned {collected} gold and gained {experience} experience.");

            Notification notification = new Notification()
            {
                ImageUrl = FarmNotificationImageUrl,
                Title = "Farm report",
                Content = $"You finished your work on the farm.",
                Gold = collected,
                Experience = experience,
                Type = NotificationType.Farm,
                Hero = hero,
            };
            await this.notificationService.AddNotification(notification);

            return collected;
        }

        public async Task CancelWork()
        {
            await this.chronometerService.NullifyWorkUntil();
        }
    }
}
