namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class MineService : IMineService
    {
        private const byte DigDuration = 5;

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

        public async Task<int> InitiateDig()
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

            return DigDuration;
        }

        public async Task<CollectedResourcesViewModel> Collect()
        {
            CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();
            Hero hero = await this.heroService.GetCurrentHero();
            HeroAmulet heroAmulet = hero.EquippedSet.Amulet;

            if (hero.WorkStatus != WorkStatus.Mine)
            {
                throw new FarmHeroesException(
                    "You haven't been working in the mines or are still working there.",
                    "Wait for your work to finish or cancel your current work to start digging.",
                    "/Mine");
            }

            Random random = new Random();
            collectedResources.Crystals = random.Next(1, 5);

            if (heroAmulet.Name == "Crystal Digger")
            {
                double chance = random.Next(0, 100);

                if (heroAmulet.Bonus >= chance)
                {
                    collectedResources.Crystals *= 2;
                    collectedResources.AmuletActivated = true;
                }
            }

            hero.Statistics.EarnedInMines += collectedResources.Crystals;

            await this.resourcePouchService.IncreaseCurrentHeroCrystals(collectedResources.Crystals);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            return collectedResources;
        }

        public async Task CancelDig()
        {
            await this.chronometerService.NullifyWorkUntil();
        }
    }
}
