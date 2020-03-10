namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ViewModels.UserModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FarmHeroesDbContext context;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;

        public UserService(IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, FarmHeroesDbContext context, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.context = context;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public string GetUsername()
        {
            return this.httpContext.HttpContext.User?.Identity?.Name;
        }

        public async Task<ApplicationUser> GetApplicationUser()
        {
            return await this.userManager.GetUserAsync(this.httpContext.HttpContext.User);
        }

        public async Task<bool> CurrentUserHasHero()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.httpContext.HttpContext.User);

            return user.Hero != null;
        }

        public async Task BanUserByUsername(BanInputModel inputModel)
        {
            ApplicationUser user = this.context.Users.SingleOrDefault(u => u.UserName == inputModel.Username);

            if (user == null)
            {
                throw new FarmHeroesException(
                    "Hero with this username not found.",
                    "Enter a username of an existing hero.",
                    "/Administration/PlayerControl/Ban");
            }

            var lockoutResult = await this.userManager.SetLockoutEndDateAsync(user, inputModel.BanUntil);

            if (lockoutResult.Succeeded)
            {
                this.tempDataDictionaryFactory
                    .GetTempData(this.httpContext.HttpContext)
                    .Add("Alert", $"You banned {user.UserName} until {inputModel.BanUntil}.");
            }
        }

        public async Task AddUserToRole(string username, string roleName)
        {
            ApplicationUser user = this.context.Users.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                throw new FarmHeroesException(
                    "Hero with this username not found.",
                    "Enter a username of an existing hero.",
                    $"/Administration/PlayerControl/ModifyRole/{roleName}");
            }

            await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveUserFromRole(string username, string roleName)
        {
            ApplicationUser user = this.context.Users.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                throw new FarmHeroesException(
                    "Hero with this username not found.",
                    "Enter a username of an existing hero.",
                    $"/Administration/PlayerControl/ModifyRole/{roleName}");
            }

            await this.userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
