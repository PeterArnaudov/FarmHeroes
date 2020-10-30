namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.HealthModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class HealthController : BaseController
    {
        private readonly IHealthService healthService;
        private readonly IResourcePouchService resourcePouchService;

        public HealthController(
            IHealthService healthService,
            IResourcePouchService resourcePouchService)
        {
            this.healthService = healthService;
            this.resourcePouchService = resourcePouchService;
        }

        public async Task<ActionResult<HealResultModel>> HealPill()
        {
            await this.healthService.HealCurrentHero(HealthConstants.PillHealAmount, HealthConstants.PillCost);

            Health health = await this.healthService.GetHealth();

            HealResultModel result = new HealResultModel
            {
                CurrentHealth = health.Current,
                MaximumHealth = health.Maximum,
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
                Crystals = await this.resourcePouchService.GetResource(ResourceNames.Crystals),
            };

            return result;
        }

        public async Task<ActionResult<HealResultModel>> HealElixir()
        {
            await this.healthService.HealCurrentHero(HealthConstants.ElixirHealAmount, HealthConstants.ElixirCost);

            Health health = await this.healthService.GetHealth();

            HealResultModel result = new HealResultModel
            {
                CurrentHealth = health.Current,
                MaximumHealth = health.Maximum,
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
                Crystals = await this.resourcePouchService.GetResource(ResourceNames.Crystals),
            };

            return result;
        }

        public async Task<ActionResult<HealResultModel>> HealPotion()
        {
            await this.healthService.HealCurrentHeroToMaximum(HealthConstants.PotionCost);

            Health health = await this.healthService.GetHealth();

            HealResultModel result = new HealResultModel
            {
                CurrentHealth = health.Current,
                MaximumHealth = health.Maximum,
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
                Crystals = await this.resourcePouchService.GetResource(ResourceNames.Crystals),
            };

            return result;
        }
    }
}
