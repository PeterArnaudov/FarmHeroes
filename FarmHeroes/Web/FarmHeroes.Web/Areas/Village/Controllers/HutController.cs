namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.HutModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    [Authorize]
    [Area("Village")]
    public class HutController : BaseController
    {
        private readonly IBonusService bonusService;

        public HutController(IBonusService bonusService)
        {
            this.bonusService = bonusService;
        }

        public async Task<IActionResult> Index()
        {
            HutBonusesViewModel viewModel = await this.bonusService.GetBonusesViewModelForLocation<HutBonusesViewModel>("Hut");

            return this.View(viewModel);
        }

        [Route("/Village/Hut/Extend/{name}")]
        public async Task<IActionResult> Extend(string name)
        {
            name = Regex.Replace(name, "[a-z][A-Z]", m => $"{m.Value[0]} {m.Value[1]}");
            await this.bonusService.ExtendBonus(name);

            return this.Redirect("/Village/Hut");
        }
    }
}
