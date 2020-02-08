namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;

    public class MineService : IMineService
    {
        private const byte DigDuration = 1;

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly IChronometerService chronometerService;

        public MineService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, IChronometerService chronometerService)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.chronometerService = chronometerService;
        }

        public async Task InitiateDig()
        {
            await this.chronometerService.SetWorkUntil(DigDuration, WorkStatus.Mine);
        }

        public async Task<int> Collect()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Mine)
            {
                throw new Exception(); // TODO: Add message
            }

            Random random = new Random();
            int collected = random.Next(1, 5);
            hero.Statistics.EarnedInMines += collected;

            await this.resourcePouchService.IncreaseCurrentHeroCrystals(collected);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            return collected;
        }

        public async Task CancelDig()
        {
            await this.chronometerService.NullifyWorkUntil();
        }
    }
}
