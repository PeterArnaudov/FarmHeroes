namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.FarmModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FarmController : BaseController
    {
        private readonly IHeroService heroService;

        public FarmController(IHeroService heroService)
        {
            this.heroService = heroService;
        }

        [Route("/Farm")]
        public async Task<IActionResult> Farm()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (!await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Farm))
            {
                return this.Redirect($"/{hero.WorkStatus.ToString()}");
            }

            FarmViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<FarmViewModel>();

            return this.View(viewModel);
        }
    }
}