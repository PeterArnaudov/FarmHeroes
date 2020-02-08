namespace FarmHeroes.Web.Components
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Microsoft.AspNetCore.Mvc;

    public class HeroHealthViewComponent : ViewComponent
    {
        private readonly IHealthService healthService;

        public HeroHealthViewComponent(IHealthService healthService)
        {
            this.healthService = healthService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeroHealthViewComponentModel viewModel = 
                await this.healthService.GetCurrentHeroHealthViewModel<HeroHealthViewComponentModel>();

            return this.View(viewModel);
        }
    }
}
