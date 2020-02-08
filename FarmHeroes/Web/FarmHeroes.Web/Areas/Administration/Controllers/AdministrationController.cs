namespace FarmHeroes.Web.Areas.Administration.Controllers
{
    using FarmHeroes.Common;
    using FarmHeroes.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
