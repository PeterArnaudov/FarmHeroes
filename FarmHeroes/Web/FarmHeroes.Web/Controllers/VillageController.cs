namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Web.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class VillageController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}