namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.FarmModels;
    using Microsoft.AspNetCore.Mvc;

    public class FarmController : BaseController
    {
        private readonly IHeroService heroService;
        private readonly IFarmService farmService;

        public FarmController(IHeroService heroService, IFarmService farmService)
        {
            this.heroService = heroService;
            this.farmService = farmService;
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

            if (viewModel.WorkUntil < DateTime.UtcNow && hero.WorkStatus == WorkStatus.Farm)
            {
                return this.Redirect("/Farm/Result");
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Work()
        {
            await this.farmService.StartWork();

            return this.Redirect("/Farm");
        }

        public IActionResult Result()
        {
            return this.View();
        }

        public async Task<IActionResult> Collect()
        {
            await this.farmService.Collect();

            return this.Redirect("/Farm");
        }

        public async Task<IActionResult> Cancel()
        {
            await this.farmService.CancelWork();

            return this.Redirect("/Farm");
        }
    }
}