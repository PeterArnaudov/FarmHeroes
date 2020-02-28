namespace FarmHeroes.Web.ApiControllers
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HealthController : ApiController
    {
        private const int PillHealAmount = 200;
        private const int ElixirHealAmount = 3000;
        private const int PillCost = 100;
        private const int ElixirCost = 1000;
        private const int PotionCost = 15;

        private readonly IHealthService healthService;
        private readonly IResourcePouchService resourcePouchService;

        public HealthController(IHealthService healthService, IResourcePouchService resourcePouchService)
        {
            this.healthService = healthService;
            this.resourcePouchService = resourcePouchService;
        }

        [HttpGet("HealPill")]
        public async Task<ActionResult<object>> HealPill()
        {
            try
            {
                await this.healthService.HealCurrentHero(PillHealAmount, PillCost);

                Health health = await this.healthService.GetCurrentHeroHealth();
                ResourcePouch resources = await this.resourcePouchService.GetCurrentHeroResources();

                object result = new
                {
                    health.Current,
                    health.Maximum,
                    resources.Gold,
                    resources.Crystals,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpGet("HealElixir")]
        public async Task<ActionResult<object>> HealElixir()
        {
            try
            {
                await this.healthService.HealCurrentHero(ElixirHealAmount, ElixirCost);

                Health health = await this.healthService.GetCurrentHeroHealth();
                ResourcePouch resources = await this.resourcePouchService.GetCurrentHeroResources();

                object result = new
                {
                    health.Current,
                    health.Maximum,
                    resources.Gold,
                    resources.Crystals,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpGet("HealPotion")]
        public async Task<ActionResult<object>> HealPotion()
        {
            try
            {
                await this.healthService.HealCurrentHeroToMaximum(PotionCost);

                Health health = await this.healthService.GetCurrentHeroHealth();
                ResourcePouch resources = await this.resourcePouchService.GetCurrentHeroResources();

                object result = new
                {
                    health.Current,
                    health.Maximum,
                    resources.Gold,
                    resources.Crystals,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}
