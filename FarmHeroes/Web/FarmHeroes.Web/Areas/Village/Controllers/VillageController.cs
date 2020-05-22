namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Web.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("Village")]
    public class VillageController : Controller
    {
        [Route("Village")]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}