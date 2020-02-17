namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class MineService : IMineService
    {
        private const byte DigDuration = 5;

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly IChronometerService chronometerService;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor context;

        public MineService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, IChronometerService chronometerService, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor context)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.chronometerService = chronometerService;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.context = context;
        }

        public async Task InitiateDig()
        {
            Chronometer chronometer = await this.chronometerService.GetCurrentHeroChronometer();

            if (chronometer.WorkUntil != null)
            {
                throw new FarmHeroesException(
                    "You already work somewhere.",
                    "You should cancel your current work before trying to start digging.",
                    "/Mine");
            }

            await this.chronometerService.SetWorkUntil(DigDuration, WorkStatus.Mine);
        }

        public async Task<int> Collect()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Mine)
            {
                throw new FarmHeroesException(
                    "You haven't been working in the mines or are still working there.",
                    "Wait for your work to finish or cancel your current work to start digging.",
                    "/Mine");
            }

            Random random = new Random();
            int collected = random.Next(1, 5);
            hero.Statistics.EarnedInMines += collected;

            await this.resourcePouchService.IncreaseCurrentHeroCrystals(collected);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            this.tempDataDictionaryFactory
                .GetTempData(this.context.HttpContext)
                .Add("Collected", $"You collected {collected} crystals.");

            return collected;
        }

        public async Task CancelDig()
        {
            await this.chronometerService.NullifyWorkUntil();
        }
    }
}
