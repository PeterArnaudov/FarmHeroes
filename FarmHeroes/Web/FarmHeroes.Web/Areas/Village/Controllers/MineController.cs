namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.MineModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize]
    [Area("Village")]
    public class MineController : BaseController
    {
        private readonly IHeroService heroService;
        private readonly IMineService mineService;
        private readonly IChronometerService chronometerService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<MineController> stringLocalizer;
        private readonly IResourcePouchService resourcePouchService;

        public MineController(IHeroService heroService, IMineService mineService, IChronometerService chronometerService, IMapper mapper, IStringLocalizer<MineController> stringLocalizer, IResourcePouchService resourcePouchService)
        {
            this.heroService = heroService;
            this.mineService = mineService;
            this.chronometerService = chronometerService;
            this.mapper = mapper;
            this.stringLocalizer = stringLocalizer;
            this.resourcePouchService = resourcePouchService;
        }

        public async Task<IActionResult> Index()
        {
            await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Mine);

            MineViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<MineViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> StartWork()
        {
            int seconds = await this.mineService.InitiateDig();

            return this.Json(new
            {
                seconds,
                timeLeftParagraph = this.stringLocalizer["Work-Time-Left-Paragraph"].Value,
                cancelWorkParagraph = this.stringLocalizer["Cancel-Work-Paragraph"].Value,
                cancelButton = this.stringLocalizer["Cancel-Button-Text"].Value,
            });
        }

        public async Task<IActionResult> Collect()
        {
            var result = await this.mineService.Collect();

            return this.Json(new
            {
                totalCrystals = this.resourcePouchService.GetResource(ResourceNames.Crystals),
                crystals = result.Crystals,
                amuletActivated = result.AmuletActivated
                    ? this.stringLocalizer["Amulet-Activated"].Value
                    : this.stringLocalizer["Amulet-Not-Activated"].Value,
                youCollected = this.stringLocalizer["You-Collected"].Value,
                salaryParagraph = this.stringLocalizer["Current-Salary-Paragraph"].Value,
                durationParagraph = this.stringLocalizer["Work-Duration-Paragraph"].Value,
                workButton = this.stringLocalizer["Work-Button-Text"].Value,
            });
        }

        public async Task<IActionResult> CancelWork()
        {
            await this.chronometerService.NullifyWorkUntil();

            return this.Json(new
            {
                durationParagraph = this.stringLocalizer["Work-Duration-Paragraph"].Value,
                workButton = this.stringLocalizer["Work-Button-Text"].Value,
            });
        }
    }
}