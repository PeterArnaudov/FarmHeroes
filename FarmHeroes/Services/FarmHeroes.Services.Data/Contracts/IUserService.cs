namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models;

    public interface IUserService
    {
        string GetUsername();

        Task<ApplicationUser> GetApplicationUserAsync();
    }
}
