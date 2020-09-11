namespace FarmHeroes.Web.Components
{
    using System.Threading.Tasks;

    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Microsoft.AspNetCore.Mvc;

    public class SideMenuResourcesViewComponent : ViewComponent
    {
        private readonly IResourcePouchService resourcePouchService;

        public SideMenuResourcesViewComponent(IResourcePouchService resourcePouchService)
        {
            this.resourcePouchService = resourcePouchService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SideMenuResourcesViewComponentModel viewModel =
                await this.resourcePouchService.GetCurrentHeroResourcesViewModel<SideMenuResourcesViewComponentModel>();

            return this.View(viewModel);
        }
    }
}
