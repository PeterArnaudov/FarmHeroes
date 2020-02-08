﻿namespace FarmHeroes.Web.Controllers
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.BattlefieldModels;
    using FarmHeroes.Web.ViewModels.FightModels;
    using Microsoft.AspNetCore.Mvc;

    public class BattlefieldController : Controller
    {
        private readonly IHeroService heroService;
        private readonly IBattlefieldService battlefieldService;
        private readonly IFightService fightService;

        public BattlefieldController(IHeroService heroService, IBattlefieldService battlefieldService, IFightService fightService)
        {
            this.heroService = heroService;
            this.battlefieldService = battlefieldService;
            this.fightService = fightService;
        }

        [Route("/Battlefield")]
        public async Task<IActionResult> Battlefield()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (!await this.heroService.ValidateCurrentHeroLocation(WorkStatus.Battlefield))
            {
                return this.Redirect($"/{hero.WorkStatus.ToString()}");
            }

            BattlefieldViewModel viewModel = await this.heroService.GetCurrentHeroViewModel<BattlefieldViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Patrol()
        {
            await this.battlefieldService.StartPatrol();

            return this.Redirect("/Battlefield");
        }

        public async Task<IActionResult> Collect()
        {
            int collected = await this.battlefieldService.Collect();
            this.TempData["Collected"] = collected;

            return this.Redirect("/Battlefield");
        }

        [HttpPost]
        public async Task<IActionResult> GetOpponents()
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

        public async Task<IActionResult> FightLog(int id)
        {
            FightLogViewModel viewModel = await this.fightService.GetFightViewModelById<FightLogViewModel>(id);

            return this.View(viewModel);
        }
    }
}