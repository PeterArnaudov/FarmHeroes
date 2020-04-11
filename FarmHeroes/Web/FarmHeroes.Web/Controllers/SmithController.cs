namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.SmithModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class SmithController : BaseController
    {
        private readonly ISmithService smithService;
        private readonly IInventoryService inventoryService;

        public SmithController(ISmithService smithService, IInventoryService inventoryService)
        {
            this.smithService = smithService;
            this.inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            SmithViewModel viewModel = await this.inventoryService.GetCurrentHeroInventoryViewModel<SmithViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Upgrade(int id)
        {
            await this.smithService.Upgrade(id);

            return this.Redirect("/Smith");
        }

        public async Task<IActionResult> UpgradeAmulet(int id)
        {
            await this.smithService.UpgradeAmulet(id);

            return this.Redirect("/Smith");
        }
    }
}
