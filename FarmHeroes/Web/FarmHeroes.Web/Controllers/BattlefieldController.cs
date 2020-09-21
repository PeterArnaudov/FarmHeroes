namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.BattlefieldModels;
    using FarmHeroes.Web.ViewModels.FightModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize]
    public class BattlefieldController : Controller
    {
        private readonly IHeroService heroService;
        private readonly IBattlefieldService battlefieldService;
        private readonly IFightService fightService;
        private readonly IChronometerService chronometerService;
        private readonly IStringLocalizer<BattlefieldController> stringLocalizer;

        public BattlefieldController(IHeroService heroService, IBattlefieldService battlefieldService, IFightService fightService, IChronometerService chronometerService, IStringLocalizer<BattlefieldController> stringLocalizer)
        {
            this.heroService = heroService;
            this.battlefieldService = battlefieldService;
            this.fightService = fightService;
            this.chronometerService = chronometerService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index()
        {
            await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Battlefield);

            BattlefieldViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<BattlefieldViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> ResetPatrol()
        {
            await this.battlefieldService.ResetPatrolLimit();

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> GetOpponents(string attackType)
        {
            BattlefieldGetOpponentsViewModel viewModel = await this.battlefieldService
                .GetOpponentsViewModel<BattlefieldGetOpponentsViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AttackPlayer(int id)
        {
            int fightId = await this.fightService.InitiateFight(id);

            return this.Redirect($"/Battlefield/FightLog/{fightId}");
        }

        public async Task<IActionResult> AttackMonster([Range(0, 3)]int monsterLevel = 0)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Battlefield");
            }

            int fightId = await this.fightService.InitiateMonsterFight(monsterLevel);

            return this.Redirect($"/Battlefield/FightLog/{fightId}");
        }

        public async Task<IActionResult> FightLog(int id)
        {
            FightLogViewModel viewModel = await this.fightService.GetFightViewModel<FightLogViewModel>(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> StartPatrol()
        {
            int seconds = await this.battlefieldService.StartPatrol();

            return this.Json(seconds);
        }

        public async Task<IActionResult> Collect()
        {
            var result = await this.battlefieldService.Collect();

            return this.Json(new
            {
                crystals = result.Crystals,
                amuletActivated = result.AmuletActivated
                    ? this.stringLocalizer["Amulet-Activated"].Value
                    : this.stringLocalizer["Amulet-Not-Activated"].Value,
                youCollected = this.stringLocalizer["You-Collected"].Value,
                workButton = this.stringLocalizer["Work-Button-Text"].Value,
            });
        }
    }
}