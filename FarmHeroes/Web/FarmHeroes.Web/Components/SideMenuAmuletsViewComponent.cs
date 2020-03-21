namespace FarmHeroes.Web.Components
{
    using System.Threading.Tasks;

    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;

    using Microsoft.AspNetCore.Mvc;

    public class SideMenuAmuletsViewComponent : ViewComponent
    {
        private readonly IInventoryService inventoryService;

        public SideMenuAmuletsViewComponent(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SideMenuAmuletsViewComponentModel viewModel =
                await this.inventoryService.GetCurrentHeroAmuletsViewModel<SideMenuAmuletsViewComponentModel>();

            return this.View(viewModel);
        }
    }
}
