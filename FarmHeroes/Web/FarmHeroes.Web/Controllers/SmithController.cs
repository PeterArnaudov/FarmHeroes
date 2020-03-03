namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.SmithModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class SmithController : BaseController
    {
        private readonly ISmithService smithService;
        private readonly IInventoryService inventoryService;

        public SmithController(ISmithService smithService, IInventoryService inventoryService)
        {
            this.smithService = smithService;
            this.inventoryService = inventoryService;
        }

        [Route("/Smith")]
        public async Task<IActionResult> Smith()
        {
            SmithViewModel viewModel = await this.inventoryService.GetCurrentHeroInventoryViewModel<SmithViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Upgrade(int id)
        {
            await this.smithService.Upgrade(id);

            return this.Redirect("/Smith");
        }
    }
}
