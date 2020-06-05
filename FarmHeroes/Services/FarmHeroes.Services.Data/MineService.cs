namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;

    public class MineService : IMineService
    {
        private const int DigDurationInSeconds = 150;

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IStatisticsService statisticsService;
        private readonly IChronometerService chronometerService;
        private readonly IAmuletBagService amuletBagService;

        public MineService(IHeroService heroService, IResourcePouchService resourcePouchService, IStatisticsService statisticsService, IChronometerService chronometerService, IAmuletBagService amuletBagService)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.statisticsService = statisticsService;
            this.chronometerService = chronometerService;
            this.amuletBagService = amuletBagService;
        }

        public async Task<int> InitiateDig()
        {
            await this.amuletBagService.EquipAmulet("Mine");

            await this.chronometerService.SetWorkUntil(DigDurationInSeconds, WorkStatus.Mine);

            return DigDurationInSeconds;
        }

        public async Task<CollectedResourcesViewModel> Collect()
        {
            CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();
            Hero hero = await this.heroService.GetHero();
            HeroAmulet heroAmulet = hero.EquippedSet.Amulet;

            this.CheckIfHeroWorkedInMine(hero);

            Random random = new Random();
            collectedResources.Crystals = random.Next(1, 5);

            if (heroAmulet?.Name == AmuletNames.CrystalDigger)
            {
                double chance = random.Next(0, 100);

                if (heroAmulet.Bonus >= chance)
                {
                    collectedResources.Crystals *= 2;
                    collectedResources.AmuletActivated = true;
                }
            }

            hero.Statistics.EarnedInMines += collectedResources.Crystals;

            await this.resourcePouchService.IncreaseCrystals(collectedResources.Crystals);
            await this.chronometerService.NullifyWorkUntil();
            await this.statisticsService.UpdateStatistics(hero.Statistics);

            return collectedResources;
        }

        private void CheckIfHeroWorkedInMine(Hero hero)
        {
            if (hero.WorkStatus != WorkStatus.Mine || hero.Chronometer.WorkUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    MineExceptionMessages.CannotCollectRewardMessage,
                    MineExceptionMessages.CannotCollectRewardInstruction,
                    Redirects.MineRedirect);
            }
        }
    }
}
