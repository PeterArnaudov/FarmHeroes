namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.InventoryModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class InventoryController : BaseController
    {
        private readonly IInventoryService inventoryService;
        private readonly IEquipmentService equipmentService;

        public InventoryController(IInventoryService inventoryService, IEquipmentService equipmentService)
        {
            this.inventoryService = inventoryService;
            this.equipmentService = equipmentService;
        }

        [Route("/Inventory")]
        public async Task<IActionResult> Inventory()
        {
            InventoryViewModel viewModel = await this.inventoryService.GetCurrentHeroInventoryViewModel<InventoryViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Upgrade()
        {
            await this.inventoryService.Upgrade();

            return this.Redirect("/Inventory");
        }

        public async Task<IActionResult> EquipHelmet(int id)
        {
            await this.equipmentService.EquipHelmet(id);

            return this.Redirect("/Inventory");
        }

        public async Task<IActionResult> EquipArmor(int id)
        {
            await this.equipmentService.EquipArmor(id);

            return this.Redirect("/Inventory");
        }

        public async Task<IActionResult> EquipWeapon(int id)
        {
            await this.equipmentService.EquipWeapon(id);

            return this.Redirect("/Inventory");
        }

        public async Task<IActionResult> EquipShield(int id)
        {
            await this.equipmentService.EquipShield(id);

            return this.Redirect("/Inventory");
        }
    }
}
