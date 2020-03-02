namespace FarmHeroes.Web.Controllers
{
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

        [Route("/Shop/Helmets")]
        public async Task<IActionResult> HelmetShop()
        {
            ShopViewModel viewModel = await this.shopService.GetShopViewModel<ShopViewModel>("Helmet");

            return this.View(viewModel);
        }

        public async Task<IActionResult> BuyHelmet(int id)
        {
            await this.shopService.SellHelmet(id);

            return this.Redirect("/Shop/Helmets");
        }

        [Route("/Shop/Armor")]
        public async Task<IActionResult> ArmorShop()
        {
            ShopViewModel viewModel = await this.shopService.GetShopViewModel<ShopViewModel>("Armor");

            return this.View(viewModel);
        }

        public async Task<IActionResult> BuyArmor(int id)
        {
            await this.shopService.SellArmor(id);

            return this.Redirect("/Shop/Armor");
        }

        [Route("/Shop/Weapons")]
        public async Task<IActionResult> WeaponShop()
        {
            ShopViewModel viewModel = await this.shopService.GetShopViewModel<ShopViewModel>("Weapon");

            return this.View(viewModel);
        }

        public async Task<IActionResult> BuyWeapon(int id)
        {
            await this.shopService.SellWeapon(id);

            return this.Redirect("/Shop/Weapons");
        }

        [Route("/Shop/Shields")]
        public async Task<IActionResult> ShieldShop()
        {
            ShopViewModel viewModel = await this.shopService.GetShopViewModel<ShopViewModel>("Weapon");

            return this.View(viewModel);
        }

        public async Task<IActionResult> BuyShield(int id)
        {
            await this.shopService.SellShield(id);

            return this.Redirect("/Shop/Shields");
        }
    }
}
