namespace FarmHeroes.Web.Controllers
{
    using System.Threading.Tasks;

    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using Microsoft.AspNetCore.Mvc;

    public class HealthController : BaseController
    {
        private readonly IHealthService healthService;

        public HealthController(IHealthService healthService)
        {
            this.healthService = healthService;
        }

        public async Task<IActionResult> Pill()
        {
            await this.healthService.HealCurrentHero(200, 100);

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Elixir()
        {
            await this.healthService.HealCurrentHero(3000, 1000);

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Potion()
        {
            await this.healthService.HealCurrentHeroToMaximum(15);

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }
    }
}
