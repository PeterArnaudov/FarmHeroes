namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.InventoryModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class InventoryController : BaseController
    {
        private readonly IInventoryService inventoryService;
        private readonly IEquipmentService equipmentService;

        public InventoryController(IInventoryService inventoryService, IEquipmentService equipmentService)
        {
            this.inventoryService = inventoryService;
            this.equipmentService = equipmentService;
        }

        public async Task<IActionResult> Index()
        {
            InventoryViewModel viewModel = await this.inventoryService.GetCurrentHeroInventoryViewModel<InventoryViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Upgrade()
        {
            await this.inventoryService.Upgrade();

            return this.Redirect("/Inventory");
        }

        public async Task<IActionResult> Equip(int id)
        {
            await this.equipmentService.Equip(id);

            return this.Redirect("/Inventory");
        }

        public async Task<IActionResult> Trash(int id)
        {
            await this.inventoryService.Trash(id);

            return this.Redirect("/Inventory");
        }

        public async Task<ActionResult<object>> EquipAmulet(int id)
        {
            object result = await this.equipmentService.EquipAmulet(id);

            return result;
        }
    }
}
