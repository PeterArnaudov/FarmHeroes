namespace FarmHeroes.Web.Components
{
    using System.Threading.Tasks;

    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;

    using Microsoft.AspNetCore.Mvc;

    public class SideMenuBonusesViewComponent : ViewComponent
    {
        private readonly IInventoryService inventoryService;

        public SideMenuBonusesViewComponent(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SideMenuBonusesViewComponentModel viewModel =
                await this.inventoryService.GetCurrentHeroInventoryViewModel<SideMenuBonusesViewComponentModel>();

            return this.View(viewModel);
        }
    }
}
