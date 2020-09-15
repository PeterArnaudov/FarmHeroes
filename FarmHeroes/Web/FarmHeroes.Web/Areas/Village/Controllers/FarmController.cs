namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.FarmModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize]
    [Area("Village")]
    public class FarmController : BaseController
    {
        private readonly IHeroService heroService;
        private readonly IFarmService farmService;
        private readonly IChronometerService chronometerService;
        private readonly IStringLocalizer<FarmController> stringLocalizer;

        public FarmController(IHeroService heroService, IFarmService farmService, IChronometerService chronometerService, IStringLocalizer<FarmController> stringLocalizer)
        {
            this.heroService = heroService;
            this.farmService = farmService;
            this.chronometerService = chronometerService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index()
        {
            await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Farm);

            FarmViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<FarmViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> StartWork()
        {
            int seconds = await this.farmService.StartWork();

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
            return this.Json(await this.farmService.Collect());
        }

        public async Task<IActionResult> CancelWork()
        {
            await this.chronometerService.NullifyWorkUntil();

            int salary = FarmFormulas.CalculateFarmSalaryPerHour(this.heroService.GetHero().Result.Level.CurrentLevel);

            return this.Json(new
            {
                salary,
                salaryParagraph = this.stringLocalizer["Current-Salary-Paragraph"].Value,
                durationParagraph = this.stringLocalizer["Work-Duration-Paragraph"].Value,
                workButton = this.stringLocalizer["Work-Button-Text"].Value,
            });
        }
    }
}
