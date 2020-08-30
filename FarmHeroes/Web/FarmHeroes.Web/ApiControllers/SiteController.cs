namespace FarmHeroes.Web.ApiControllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;

    public class SiteController : ApiController
    {
        [HttpGet("SetLanguage/{culture}")]
        public ActionResult SetLanguage(string culture)
        {
            string[] validCultures = new[] { "en", "bg" };

            if (!validCultures.Contains(culture))
            {
                return this.BadRequest();
            }

            this.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(1),
                    IsEssential = true,  // critical settings to apply new culture
                    Path = "/",
                    HttpOnly = false,
                });

            return this.Ok();
        }
    }
}
