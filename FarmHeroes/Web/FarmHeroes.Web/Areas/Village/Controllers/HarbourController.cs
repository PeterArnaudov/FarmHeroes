namespace FarmHeroes.Web.Areas.Village.Controllers
{
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Controllers;
    using FarmHeroes.Web.ViewModels.HarbourModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Area("Village")]
    public class HarbourController : BaseController
    {
        private readonly IHeroService heroService;
        private readonly IHarbourService harbourService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IPremiumFeaturesService premiumFeaturesService;

        public HarbourController(
            IHeroService heroService,
            IHarbourService harbourService,
            IResourcePouchService resourcePouchService,
            IPremiumFeaturesService premiumFeaturesService)
        {
            this.heroService = heroService;
            this.harbourService = harbourService;
            this.resourcePouchService = resourcePouchService;
            this.premiumFeaturesService = premiumFeaturesService;
        }

        public async Task<IActionResult> Index()
        {
            HarbourViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<HarbourViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> SetSail()
        {
            int seconds = await this.harbourService.SetSail();

            return this.Json(seconds);
        }

        public async Task<IActionResult> BuyVessels(string vessel)
        {
            await this.harbourService.BuyFishingVessel(vessel);

            BuyVesselsResultModel resultModel = new BuyVesselsResultModel()
            {
                Crystals = await this.resourcePouchService.GetResource(ResourceNames.Crystals),
                Vessels = await this.resourcePouchService.GetResource(vessel),
            };

            return this.Json(resultModel);
        }

        public async Task<IActionResult> ToggleManager()
        {
            await this.premiumFeaturesService.ToggleFeature(PremiumFeatureNames.HarbourManager);

            return this.RedirectToAction("Index");
        }
    }
}
