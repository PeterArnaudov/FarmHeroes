namespace FarmHeroes.Web.Controllers
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.DungeonModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [LevelRequirement(RequiredLevels.DungeonRequiredLevel)]
    public class DungeonController : BaseController
    {
        private readonly IDungeonService dungeonService;

        public DungeonController(IDungeonService dungeonService)
        {
            this.dungeonService = dungeonService;
        }

        public async Task<IActionResult> Index()
        {
            DungeonIndexViewModel viewModel = await this.dungeonService.GetDungeonViewModel<DungeonIndexViewModel>();

            if (viewModel.WorkStatus == WorkStatus.Dungeon || viewModel.WorkStatus == WorkStatus.DungeonIdle)
            {
                return this.RedirectToAction("Walking");
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Walking()
        {
            DungeonWalkingViewModel viewModel = await this.dungeonService.GetDungeonViewModel<DungeonWalkingViewModel>();

            if (viewModel.ChronometerWorkUntil == null)
            {
                return this.RedirectToAction("Index");
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> BuyKey()
        {
            await this.dungeonService.BuyKey();

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Enter()
        {
            await this.dungeonService.StartDungeon();

            return this.RedirectToAction("Walking");
        }

        public async Task<IActionResult> NextFloor()
        {
            await this.dungeonService.GoToNextFloor();

            return this.RedirectToAction("Walking");
        }

        public async Task<IActionResult> WalkOnFloor()
        {
            await this.dungeonService.WalkOnFloor();

            return this.RedirectToAction("Walking");
        }

        public async Task<IActionResult> Exit()
        {
            await this.dungeonService.EndDungeon();

            return this.RedirectToAction("Index");
        }
    }
}
