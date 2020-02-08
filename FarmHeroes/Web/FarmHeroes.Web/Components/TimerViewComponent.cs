namespace FarmHeroes.Web.Components
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Microsoft.AspNetCore.Mvc;

    public class TimerViewComponent : ViewComponent
    {
        public TimerViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string domElementId, DateTime? dateTime)
        {
            TimerViewComponentModel viewModel = new TimerViewComponentModel
            {
                TimeSpan = dateTime - DateTime.UtcNow,
                DomElementId = domElementId,
            };

            if (viewModel.TimeSpan == null)
            {
                viewModel.TimeSpan = new TimeSpan(0, 0, 0);
            }

            return this.View(viewModel);
        }
    }
}
