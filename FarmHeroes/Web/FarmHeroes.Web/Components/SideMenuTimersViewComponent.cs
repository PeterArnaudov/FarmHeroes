namespace FarmHeroes.Web.Components
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Microsoft.AspNetCore.Mvc;

    public class SideMenuTimersViewComponent : ViewComponent
    {
        private readonly IChronometerService chronometerService;

        public SideMenuTimersViewComponent(IChronometerService chronometerService)
        {
            this.chronometerService = chronometerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SideMenuTimersViewComponentModel viewModel =
                await this.chronometerService.GetCurrentHeroChronometerViewModel<SideMenuTimersViewComponentModel>();

            return this.View(viewModel);
        }
    }
}
