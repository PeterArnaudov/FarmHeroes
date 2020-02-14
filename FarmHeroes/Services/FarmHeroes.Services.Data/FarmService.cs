namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;

    public class FarmService : IFarmService
    {
        private const int WorkDurationInMinutes = 240;
        private const int WorkDurationInHours = WorkDurationInMinutes / 60;

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly ILevelService levelService;
        private readonly IChronometerService chronometerService;

        public FarmService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, ILevelService levelService, IChronometerService chronometerService)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.levelService = levelService;
            this.chronometerService = chronometerService;
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

            return collected;
        }

        public async Task CancelWork()
        {
            await this.chronometerService.NullifyWorkUntil();
        }
    }
}
