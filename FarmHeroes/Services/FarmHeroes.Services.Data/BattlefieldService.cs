namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;

    public class BattlefieldService : IBattlefieldService
    {
        private const int PatrolDurationInSeconds = 600;
        private const string PatrolNotificationImageUrl = "/images/notifications/patrol-notification.png";

        private readonly IHeroService heroService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly INotificationService notificationService;
        private readonly FarmHeroesDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor httpContext;
        private readonly IDailyLimitsService dailyLimitsService;

        public BattlefieldService(IHeroService heroService, IChronometerService chronometerService, ILevelService levelService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, INotificationService notificationService, FarmHeroesDbContext dbContext, IMapper mapper, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContext, IDailyLimitsService dailyLimitsService)
        {
            this.heroService = heroService;
            this.chronometerService = chronometerService;
            this.levelService = levelService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.notificationService = notificationService;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.httpContext = httpContext;
            this.dailyLimitsService = dailyLimitsService;
        }

        public async Task<int> StartPatrol()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.DailyLimits.PatrolsDone >= hero.DailyLimits.PatrolLimit)
            {
                throw new FarmHeroesException(
                    "You cannot go on a patrol.",
                    "You've already been on patrol the maximum allowed times today.",
                    "/Battlefield");
            }

            int durationInSeconds = PatrolDurationInSeconds;

            Random random = new Random();

            if (hero.EquippedSet.Amulet?.Name == "Speedster")
            {
                double chanceNeeded = random.Next(0, 100);

                if (hero.EquippedSet.Amulet.Bonus >= chanceNeeded)
                {
                    durationInSeconds = 10;
                }
            }

            await this.chronometerService.SetWorkUntil(durationInSeconds, WorkStatus.Battlefield);

            return durationInSeconds;
        }

        public async Task<CollectedResourcesViewModel> Collect()
        {
            CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Battlefield)
            {
                throw new FarmHeroesException(
                    "You haven't been on patrol or are still patrolling.",
                    "You have to cancel or finish your patrol before trying to collect your earnings.",
                    "/Battlefield");
            }

            collectedResources.Experience = BattlefieldFormulas.CalculatePatrolExperience(hero.Level.CurrentLevel);
            collectedResources.Gold = BattlefieldFormulas.CalculatePatrolGold(hero.Level.CurrentLevel);

            hero.Statistics.EarnedOnPatrol += collectedResources.Gold;
            hero.DailyLimits.PatrolsDone++;

            await this.levelService.GiveCurrentHeroExperience(collectedResources.Experience);
            await this.resourcePouchService.IncreaseCurrentHeroGold(collectedResources.Gold);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);
            await this.dailyLimitsService.UpdateDailyLimits(hero.DailyLimits);

            Notification notification = new Notification()
            {
                ImageUrl = PatrolNotificationImageUrl,
                Title = "Patrol report",
                Content = $"You finished your patrol.",
                Gold = collectedResources.Gold,
                Experience = collectedResources.Experience,
                Type = NotificationType.Patrol,
                Hero = hero,
            };
            await this.notificationService.AddNotification(notification);

            return collectedResources;
        }

        public async Task<Hero[]> GetOpponents()
        {
            Hero attacker = await this.heroService.GetCurrentHero();

            if (attacker.WorkStatus != WorkStatus.Idle)
            {
                throw new FarmHeroesException(
                     "You cannot attack while working.",
                     "You have to cancel or finish your work before trying to attack.",
                     "/Battlefield");
            }
            else if (attacker.Chronometer.CannotAttackHeroUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    "You cannot attack right now.",
                    "You have to wait until you can initiate a fight again.",
                    "/Battlefield");
            }

            int level = attacker.Level.CurrentLevel;

            Hero[] heroes = await this.dbContext.Heroes
                .Where(x => (x.Level.CurrentLevel <= level + 3 && x.Level.CurrentLevel >= level - 3)
                    && x.Id != attacker.Id
                    && x.Fraction != attacker.Fraction
                    && x.Chronometer.CannotBeAttackedUntil < DateTime.UtcNow
                    && x.UserId != null)
                .ToArrayAsync();

            if (heroes.Length == 0)
            {
                throw new FarmHeroesException(
                    "There are no heroes that you can attack right now.",
                    "There might be no heroes in your level range or all are with attack immunity.",
                    "/Battlefield");
            }

            Random random = new Random();
            Hero[] opponents = new Hero[]
            {
                heroes[random.Next(0, heroes.Length)],
                heroes[random.Next(0, heroes.Length)],
                heroes[random.Next(0, heroes.Length)],
            }
                .Distinct()
                .ToArray();

            return opponents;
        }

        public async Task<TViewModel> GetOpponentsViewModel<TViewModel>()
        {
            Hero[] opponents = await this.GetOpponents();
            TViewModel viewModel = this.mapper.Map<TViewModel>(opponents);

            return viewModel;
        }
    }
}
