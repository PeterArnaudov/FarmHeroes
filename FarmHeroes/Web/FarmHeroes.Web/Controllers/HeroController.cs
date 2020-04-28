namespace FarmHeroes.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class HeroController : BaseController
    {
        private readonly IHeroService heroService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FarmHeroesDbContext context;

        public HeroController(IHeroService heroService, IUserService userService, UserManager<ApplicationUser> userManager, FarmHeroesDbContext context)
        {
            this.heroService = heroService;
            this.userService = userService;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            HeroOverviewViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<HeroOverviewViewModel>();

            if (viewModel == null)
            {
                return this.Redirect("/Hero/Create");
            }

            return this.View(viewModel);
        }

        [Route("/Hero/{id}")]
        public async Task<IActionResult> Hero(int id)
        {
            HeroViewModel viewModel = await this.heroService.GetHeroViewModelById<HeroViewModel>(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            if (await this.userService.CurrentUserHasHero())
            {
                return this.Redirect("/Hero");
            }

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

            return this.Redirect("/Hero");
        }
    }
}