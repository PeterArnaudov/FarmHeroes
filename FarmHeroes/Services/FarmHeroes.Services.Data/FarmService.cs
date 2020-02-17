namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class FarmService : IFarmService
    {
        private const int WorkDurationInMinutes = 240;
        private const int WorkDurationInHours = WorkDurationInMinutes / 60;

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly ILevelService levelService;
        private readonly IChronometerService chronometerService;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor context;

        public FarmService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, ILevelService levelService, IChronometerService chronometerService, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor context)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.levelService = levelService;
            this.chronometerService = chronometerService;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.context = context;
        }

        public async Task StartWork()
        {
            Chronometer chronometer = await this.chronometerService.GetCurrentHeroChronometer();

            if (chronometer.WorkUntil != null)
            {
                throw new Exception("You already work somewhere.");
            }

            await this.chronometerService.SetWorkUntil(WorkDurationInMinutes, WorkStatus.Farm);
        }

        public async Task<int> Collect()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.WorkStatus != WorkStatus.Farm)
            {
                throw new Exception("You haven't been working on the farm or are still working there.");
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

            return collected;
        }

        public async Task CancelWork()
        {
            await this.chronometerService.NullifyWorkUntil();
        }
    }
}
