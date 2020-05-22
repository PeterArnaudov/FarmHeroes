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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("Village")]
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

        public async Task<IActionResult> Index()
        {
            await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Mine);

            MineViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<MineViewModel>();

            return this.View(viewModel);
        }
    }
}