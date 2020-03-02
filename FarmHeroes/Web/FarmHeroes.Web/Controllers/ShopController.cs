namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.InventoryModels;
    using FarmHeroes.Web.ViewModels.ShopModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class ShopController : BaseController
    {
        private readonly IShopService shopService;

        public ShopController(IShopService shopService)
        {
            this.shopService = shopService;
        }

        [Route("/Shop/{type}")]
        public async Task<IActionResult> Shop(EquipmentType type)
        {
            ShopViewModel viewModel = await this.shopService.GetShopViewModel<ShopViewModel>(type);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Buy(int id)
        {
            string type = await this.shopService.Sell(id);

            return this.Redirect($"/Shop/{type}");
        }
    }
}
