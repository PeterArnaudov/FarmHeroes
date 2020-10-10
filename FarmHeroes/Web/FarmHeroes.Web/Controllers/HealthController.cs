namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
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

        public async Task<ActionResult<object>> HealPill()
        {
            await this.healthService.HealCurrentHero(HealthConstants.PillHealAmount, HealthConstants.PillCost);

            Health health = await this.healthService.GetHealth();
            ResourcePouch resources = await this.resourcePouchService.GetResourcePouch();

            object result = new
            {
                health.Current,
                health.Maximum,
                resources.Gold,
                resources.Crystals,
            };

            return result;
        }

        public async Task<ActionResult<object>> HealElixir()
        {
            await this.healthService.HealCurrentHero(HealthConstants.ElixirHealAmount, HealthConstants.ElixirCost);

            Health health = await this.healthService.GetHealth();
            ResourcePouch resources = await this.resourcePouchService.GetResourcePouch();

            object result = new
            {
                health.Current,
                health.Maximum,
                resources.Gold,
                resources.Crystals,
            };

            return result;
        }

        public async Task<ActionResult<object>> HealPotion()
        {
            await this.healthService.HealCurrentHeroToMaximum(HealthConstants.PotionCost);

            Health health = await this.healthService.GetHealth();
            ResourcePouch resources = await this.resourcePouchService.GetResourcePouch();

            object result = new
            {
                health.Current,
                health.Maximum,
                resources.Gold,
                resources.Crystals,
            };

            return result;
        }
    }
}
