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
    using Microsoft.EntityFrameworkCore;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Constants;

    public class BattlefieldService : IBattlefieldService
    {
        private const int PatrolDurationInSeconds = 600;
        private const int PatrolDurationInSecondsWithSpeedster = 10;
        private const int PatrolResetPrice = 100;
        private const string PatrolNotificationTitle = "Patrol report";
        private const string PatrolNotificationContent = "You finished your patrol.";
        private const string PatrolNotificationImageUrl = "/images/notifications/patrol-notification.png";

        private readonly IHeroService heroService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly INotificationService notificationService;
        private readonly IDailyLimitsService dailyLimitsService;
        private readonly IAmuletBagService amuletBagService;
        private readonly FarmHeroesDbContext dbContext;
        private readonly IMapper mapper;

        public BattlefieldService(IHeroService heroService, IChronometerService chronometerService, ILevelService levelService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, INotificationService notificationService, FarmHeroesDbContext dbContext, IMapper mapper, IDailyLimitsService dailyLimitsService, IAmuletBagService amuletBagService)
        {
            this.heroService = heroService;
            this.chronometerService = chronometerService;
            this.levelService = levelService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.notificationService = notificationService;
            this.dailyLimitsService = dailyLimitsService;
            this.amuletBagService = amuletBagService;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<int> StartPatrol()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfPatrolLimitIsReached(hero);

            await this.amuletBagService.EquipAmulet("Patrol");

            int durationInSeconds = this.GetPatrolDurationInSeconds(hero);
            await this.chronometerService.SetWorkUntil(durationInSeconds, WorkStatus.Battlefield);

            return durationInSeconds;
        }

        public async Task ResetPatrolLimit()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfPatrolResetsLimitIsReached(hero);

            await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, PatrolResetPrice);

            hero.DailyLimits.PatrolResets = 1;
            hero.DailyLimits.PatrolsDone = 0;

            await this.dailyLimitsService.UpdateDailyLimits(hero.DailyLimits);
        }

        public async Task<CollectedResourcesViewModel> Collect()
        {
            CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();
            Hero hero = await this.heroService.GetHero();

            this.CheckIfHeroIsPatrolling(hero);

            collectedResources.Experience = BattlefieldFormulas.CalculatePatrolExperience(hero.Level.CurrentLevel);
            collectedResources.Gold = BattlefieldFormulas.CalculatePatrolGold(hero.Level.CurrentLevel);

            hero.Statistics.EarnedOnPatrol += collectedResources.Gold;
            hero.DailyLimits.PatrolsDone++;

            await this.levelService.GiveHeroExperience(collectedResources.Experience);
            await this.resourcePouchService.IncreaseResource(ResourceNames.Gold, collectedResources.Gold);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);
            await this.dailyLimitsService.UpdateDailyLimits(hero.DailyLimits);

            Notification notification = new Notification()
            {
                ImageUrl = PatrolNotificationImageUrl,
                Title = PatrolNotificationTitle,
                Content = PatrolNotificationContent,
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
            Hero attacker = await this.heroService.GetHero();

            this.CheckIfHeroIsFree(attacker);
            this.CheckIfHeroIsResting(attacker);

            int level = attacker.Level.CurrentLevel;

            Hero[] heroes = await this.dbContext.Heroes
                .Where(x => x.Level.CurrentLevel <= level + 3 && x.Level.CurrentLevel >= level - 3
                    && x.Id != attacker.Id
                    && x.Fraction != attacker.Fraction
                    && x.Chronometer.CannotBeAttackedUntil < DateTime.UtcNow
                    && x.ApplicationUserId != null)
                .ToArrayAsync();

            if (heroes.Length == 0)
            {
                throw new FarmHeroesException(
                    BattlefieldExceptionMessages.NoEnemiesAvailableMessage,
                    BattlefieldExceptionMessages.NoEnemiesAvailableInstruction,
                    Redirects.BattlefieldRedirect);
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

        private void CheckIfPatrolLimitIsReached(Hero hero)
        {
            if (hero.DailyLimits.PatrolsDone >= hero.DailyLimits.PatrolLimit)
            {
                throw new FarmHeroesException(
                    BattlefieldExceptionMessages.CannotPatrolMessage,
                    BattlefieldExceptionMessages.PatrolLimitInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfPatrolResetsLimitIsReached(Hero hero)
        {
            if (hero.DailyLimits.PatrolResets == hero.DailyLimits.PatrolResetsLimit)
            {
                throw new FarmHeroesException(
                    BattlefieldExceptionMessages.CannotResetPatrolMessage,
                    BattlefieldExceptionMessages.CannotResetPatrolInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private int GetPatrolDurationInSeconds(Hero hero)
        {
            int durationInSeconds = PatrolDurationInSeconds;

            Random random = new Random();

            if (hero.EquippedSet.Amulet?.Name == AmuletNames.Speedster)
            {
                double chanceNeeded = random.Next(0, 100);

                if (hero.EquippedSet.Amulet.Bonus >= chanceNeeded)
                {
                    durationInSeconds = PatrolDurationInSecondsWithSpeedster;
                }
            }

            return durationInSeconds;
        }

        private void CheckIfHeroIsPatrolling(Hero hero)
        {
            if (hero.WorkStatus != WorkStatus.Battlefield || hero.Chronometer.WorkUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    BattlefieldExceptionMessages.CannotCollectRewardMessage,
                    BattlefieldExceptionMessages.CannotCollectRewardInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroIsFree(Hero hero)
        {
            if (hero.WorkStatus != WorkStatus.Idle)
            {
                throw new FarmHeroesException(
                     BattlefieldExceptionMessages.CannotAttackMessage,
                     BattlefieldExceptionMessages.CannotAttackWhileWorkingInstruction,
                     Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroIsResting(Hero hero)
        {
            if (hero.Chronometer.CannotAttackHeroUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    BattlefieldExceptionMessages.CannotAttackMessage,
                    BattlefieldExceptionMessages.CannotAttackWhileRestingInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }
    }
}
