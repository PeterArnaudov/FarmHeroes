namespace FarmHeroes.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HeroController : BaseController
    {
        private readonly IHeroService heroService;

        public HeroController(IHeroService heroService)
        {
            this.heroService = heroService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HeroCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Hero/Create");
            }

            await this.heroService.CreateHero(inputModel);

            return this.Redirect("/Hero/Overview");
        }

        public async Task<IActionResult> Overview()
        {
            HeroOverviewViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<HeroOverviewViewModel>();

            if (viewModel == null)
            {
                return this.Redirect("/Hero/Create");
            }

            return this.View(viewModel);
        }
    }
}