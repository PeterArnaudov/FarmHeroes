namespace FarmHeroes.Web.Components
{
    using System.Threading.Tasks;

    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Microsoft.AspNetCore.Mvc;

    public class HeroResourcesViewComponent : ViewComponent
    {
        private readonly IResourcePouchService resourcesService;

        public HeroResourcesViewComponent(IResourcePouchService resourcePouchService)
        {
            this.resourcesService = resourcePouchService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeroResourcesViewComponentModel viewModel =
                await this.resourcesService.GetCurrentHeroResourcesViewModel<HeroResourcesViewComponentModel>();

            return this.View(viewModel);
        }
    }
}
