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
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.EntityFrameworkCore;

    public class BattlefieldService : IBattlefieldService
    {
        private const int PatrolDurationInMinutes = 10;

        private readonly IHeroService heroService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public BattlefieldService(IHeroService heroService, IChronometerService chronometerService, ILevelService levelService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.heroService = heroService;
            this.chronometerService = chronometerService;
            this.levelService = levelService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task StartPatrol()
        {
            await this.chronometerService.SetWorkUntil(PatrolDurationInMinutes, WorkStatus.Battlefield);
        }

        public async Task<int> Collect()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Battlefield)
            {
                throw new Exception(); // TODO: Add message
            }

            int experience = BattlefieldFormulas.CalculateExperience(hero.Level.CurrentLevel);
            int collected = BattlefieldFormulas.CalculatePatrolGold(hero.Level.CurrentLevel);

            hero.Statistics.EarnedOnPatrol += collected;

            await this.levelService.GiveCurrentHeroExperience(experience);
            await this.resourcePouchService.IncreaseCurrentHeroGold(collected);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            return collected;
        }

        public async Task<Hero[]> GetOpponents()
        {
            Hero attacker = await this.heroService.GetCurrentHero();

            if (attacker.WorkStatus != WorkStatus.Idle)
            {
                throw new Exception("You cannot attack while working.");
            }
            else if (attacker.Chronometer.CannotAttackHeroUntil > DateTime.UtcNow)
            {
                throw new Exception("You cannot attack right now.");
            }

            int level = attacker.Level.CurrentLevel;

            Hero[] heroes = await this.context.Heroes
                .Where(x => (x.Level.CurrentLevel <= level + 3 && x.Level.CurrentLevel >= level - 3)
                    && x.Id != attacker.Id
                    && x.Fraction != attacker.Fraction
                    && x.Chronometer.CannotBeAttackedUntil < DateTime.UtcNow)
                .ToArrayAsync();

            Random random = new Random();
            Hero[] opponents = new Hero[]
            {
                heroes[random.Next(0, heroes.Length)],
                heroes[random.Next(0, heroes.Length)],
                heroes[random.Next(0, heroes.Length)],
            };

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
