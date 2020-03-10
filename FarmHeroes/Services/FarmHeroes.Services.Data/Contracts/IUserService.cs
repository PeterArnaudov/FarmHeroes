namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models;
    using FarmHeroes.Web.ViewModels.UserModels;

    public interface IUserService
    {
        string GetUsername();

        Task<ApplicationUser> GetApplicationUser();

        Task<bool> CurrentUserHasHero();

        Task BanUserByUsername(BanInputModel inputModel);

        Task AddUserToRole(string username, string roleName);

        Task RemoveUserFromRole(string username, string roleName);
    }
}
