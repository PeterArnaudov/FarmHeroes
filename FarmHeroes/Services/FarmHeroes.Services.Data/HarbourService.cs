namespace FarmHeroes.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.EntityFrameworkCore;

    public class HarbourService : IHarbourService
    {
        private const int SailDurationInSeconds = 3600;
        private const int RaftMinimumCatch = 20;
        private const int RaftMaximumCatch = 30;
        private const int BoatMinimumCatch = 40;
        private const int BoatMaximumCatch = 65;
        private const int ShipMinimumCatch = 75;
        private const int ShipMaximumCatch = 100;
        private const int SubmarineMinimumCatch = 200;
        private const int SubmarineMaximumCatch = 300;
        private const int BoatCost = 50;
        private const int ShipCost = 100;
        private const int SubmarineCost = 250;
        private const int VesselsPerPurchase = 50;
        private const int HarbourManagerCost = 10;
        private const string HarbourNotificationImageUrl = "/images/notifications/fish-notification.png";
        private const string HarbourNotificationTitle = "Fishing report";
        private const string HarbourNotificationContent = "Your fishing vessel returned.";

        private readonly IResourcePouchService resourcePouchService;
        private readonly IChronometerService chronometerService;
        private readonly INotificationService notificationService;
        private readonly IPremiumFeaturesService premiumFeaturesService;
        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;

        public HarbourService(
            IResourcePouchService resourcePouchService,
            IChronometerService chronometerService,
            INotificationService notificationService,
            IPremiumFeaturesService premiumFeaturesService,
            IHeroService heroService,
            FarmHeroesDbContext context)
        {
            this.resourcePouchService = resourcePouchService;
            this.chronometerService = chronometerService;
            this.notificationService = notificationService;
            this.premiumFeaturesService = premiumFeaturesService;
            this.heroService = heroService;
            this.context = context;
        }

        public async Task<int> BuyFishingVessel(string vessel)
        {
            int cost = this.DetermineFishingVesselCost(vessel);

            await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, cost);
            await this.resourcePouchService.IncreaseResource(vessel, VesselsPerPurchase);

            return VesselsPerPurchase;
        }

        public async Task<int> SetSail(int id = 0)
        {
            await this.chronometerService.SetSailingUntil(SailDurationInSeconds, id: id);

            return SailDurationInSeconds;
        }

        public async Task<int> Collect(int id = 0)
        {
            Chronometer chronometer = await this.chronometerService.GetChronometer(id);

            bool isFinished = chronometer.SailingUntil.HasValue && chronometer.SailingUntil.Value < DateTime.UtcNow;
            if (!isFinished)
            {
                return 0;
            }

            string fishingVessel = await this.DetermineFishingVessel(id);

            int minimumCatch = this.DetermineMinimumCatch(fishingVessel);
            int maximumCatch = this.DetermineMaximumCatch(fishingVessel);

            if (!string.IsNullOrEmpty(fishingVessel))
            {
                await this.resourcePouchService.DecreaseResource(fishingVessel, 1, id);
            }

            int fishCaught = HarbourFormulas.CalculateFishCatch(minimumCatch, maximumCatch);
            await this.resourcePouchService.IncreaseResource(ResourceNames.Fish, fishCaught, id);
            await this.chronometerService.SetSailingUntil(0, setToNull: true, id: id);

            Notification notification = new Notification()
            {
                ImageUrl = HarbourNotificationImageUrl,
                Title = HarbourNotificationTitle,
                Content = HarbourNotificationContent,
                Fish = fishCaught,
                Type = NotificationType.Harbour,
                Hero = await this.heroService.GetHero(id),
            };
            await this.notificationService.AddNotification(notification);

            await this.ManagerSetSail(id);

            return fishCaught;
        }

        public async Task ManagerSetSail(int id = 0)
        {
            bool isManagerEnabled = await this.premiumFeaturesService.CheckIfEnabled(PremiumFeatureNames.HarbourManager, id);
            Chronometer chronometer = await this.chronometerService.GetChronometer(id);

            if (!chronometer.SailingUntil.HasValue && isManagerEnabled)
            {
                bool isPaid = await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, HarbourManagerCost, id);

                if (isPaid)
                {
                    await this.SetSail(id);
                }
                else
                {
                    await this.premiumFeaturesService.ToggleFeature(PremiumFeatureNames.HarbourManager, id);
                }
            }
        }

        public async Task CollectAll()
        {
            int[] collectable = await this.context
                .Chronometers
                .Where(x => x.SailingUntil.HasValue && x.SailingUntil.Value < DateTime.UtcNow)
                .Select(x => x.Id)
                .ToArrayAsync();

            foreach (var id in collectable)
            {
                await this.Collect(id);
            }
        }

        private async Task<string> DetermineFishingVessel(int id = 0)
        {
            ResourcePouch resources = await this.resourcePouchService.GetResourcePouch(id);

            return resources.Submarines > 0 ? ResourceNames.Submarines
                : resources.Ships > 0 ? ResourceNames.Ships
                : resources.Boats > 0 ? ResourceNames.Boats
                : string.Empty;
        }

        private int DetermineMinimumCatch(string fishingVessel)
        {
            return fishingVessel == ResourceNames.Submarines ? SubmarineMinimumCatch
                : fishingVessel == ResourceNames.Ships ? ShipMinimumCatch
                : fishingVessel == ResourceNames.Boats ? BoatMinimumCatch
                : RaftMinimumCatch;
        }

        private int DetermineMaximumCatch(string fishingVessel)
        {
            return fishingVessel == ResourceNames.Submarines ? SubmarineMaximumCatch
                : fishingVessel == ResourceNames.Ships ? ShipMaximumCatch
                : fishingVessel == ResourceNames.Boats ? BoatMaximumCatch
                : RaftMaximumCatch;
        }

        private int DetermineFishingVesselCost(string fishingVessel)
        {
            return fishingVessel == ResourceNames.Submarines ? SubmarineCost
                : fishingVessel == ResourceNames.Ships ? ShipCost
                : fishingVessel == ResourceNames.Boats ? BoatCost
                : default;
        }
    }
}
