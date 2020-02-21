namespace FarmHeroes.Services.Data
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor context;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public string GetUsername()
        {
            return this.context.HttpContext.User?.Identity?.Name;
        }

        public async Task<ApplicationUser> GetApplicationUser()
        {
            return await this.userManager.GetUserAsync(this.context.HttpContext.User);
        }

        public async Task<bool> CurrentUserHasHero()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.context.HttpContext.User);

            return user.Hero != null;
        }
    }
}
