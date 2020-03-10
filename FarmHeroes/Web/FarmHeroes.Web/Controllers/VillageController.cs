namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Web.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class VillageController : Controller
    {
        [Route("/Village")]
        public IActionResult Village()
        {
            return this.View();
        }
    }
}