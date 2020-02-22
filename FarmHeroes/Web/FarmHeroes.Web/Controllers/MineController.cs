namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.MineModels;

    using Microsoft.AspNetCore.Mvc;

    public class MineController : BaseController
    {
        private readonly IHeroService heroService;
        private readonly IMineService mineService;
        private readonly IMapper mapper;

        public MineController(IHeroService heroService, IMineService mineService, IMapper mapper)
        {
            this.heroService = heroService;
            this.mineService = mineService;
            this.mapper = mapper;
        }

        [Route("/Mine")]
        public async Task<IActionResult> Mine()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (!await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Mine))
            {
                return this.Redirect($"/{hero.WorkStatus.ToString()}");
            }

            MineViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<MineViewModel>();

            if (viewModel.WorkUntil < DateTime.UtcNow && hero.WorkStatus == WorkStatus.Mine)
            {
                return this.Redirect("/Mine/Result");
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Dig()
        {
            await this.mineService.InitiateDig();

            return this.Redirect("/Mine");
        }

        public IActionResult Result()
        {
            return this.View();
        }

        public async Task<IActionResult> Collect()
        {
            await this.mineService.Collect();

            return this.Redirect("/Mine");
        }

        public async Task<IActionResult> Cancel()
        {
            await this.mineService.CancelDig();

            return this.Redirect("/Mine");
        }
    }
}