﻿namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;

    public class MineService : IMineService
    {
        private const int DigDurationInSeconds = 300;

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
            await this.chronometerService.SetWorkUntil(DigDurationInSeconds, WorkStatus.Mine);

            return DigDurationInSeconds;
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

            if (heroAmulet?.Name == "Crystal Digger")
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
    }
}
