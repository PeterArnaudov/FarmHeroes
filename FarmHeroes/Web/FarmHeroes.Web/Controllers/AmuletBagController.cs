namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.AmuletBagModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class AmuletBagController : BaseController
    {
        private readonly IAmuletBagService amuletBagService;

        public AmuletBagController(IAmuletBagService amuletBagService)
        {
            this.amuletBagService = amuletBagService;
        }

        public async Task<IActionResult> Index()
        {
            AmuletBagViewModel viewModel = await this.amuletBagService.GetAmuletBagViewModel();

            return this.View(viewModel);
        }

        public async Task<IActionResult> ExtendRent()
        {
            await this.amuletBagService.ExtendRent();

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Purchase()
        {
            await this.amuletBagService.Purchase();

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Set(AmuletBagViewModel inputModel)
        {
            await this.amuletBagService.Set(inputModel);

            return this.RedirectToAction("Index");
        }
    }
}
