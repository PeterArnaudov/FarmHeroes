namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;

    public class BattlefieldService : IBattlefieldService
    {
        private const int PatrolDurationInMinutes = 10;

        private readonly IHeroService heroService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly FarmHeroesDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor httpContext;

        public BattlefieldService(IHeroService heroService, IChronometerService chronometerService, ILevelService levelService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, FarmHeroesDbContext dbContext, IMapper mapper, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContext)
        {
            this.heroService = heroService;
            this.chronometerService = chronometerService;
            this.levelService = levelService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.httpContext = httpContext;
        }

        public async Task StartPatrol()
        {
            Chronometer chronometer = await this.chronometerService.GetCurrentHeroChronometer();

            await this.chronometerService.SetWorkUntil(PatrolDurationInMinutes, WorkStatus.Battlefield);
        }

        public async Task<int> Collect()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Battlefield)
            {
                throw new FarmHeroesException(
                    "You haven't been on patrol or are still patrolling.",
                    "You have to cancel or finish your patrol before trying to collect your earnings.",
                    "/Battlefield");
            }

            int experience = BattlefieldFormulas.CalculateExperience(hero.Level.CurrentLevel);
            int collected = BattlefieldFormulas.CalculatePatrolGold(hero.Level.CurrentLevel);

            hero.Statistics.EarnedOnPatrol += collected;

            await this.levelService.GiveCurrentHeroExperience(experience);
            await this.resourcePouchService.IncreaseCurrentHeroGold(collected);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Collected", $"You earned {collected} gold and gained {experience} experience.");

            return collected;
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
                    && x.Chronometer.CannotBeAttackedUntil < DateTime.UtcNow)
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
