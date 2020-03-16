namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IBonusService
    {
        Task<T> GetBonusesViewModelForLocation<T>(string location);

        Task ExtendBonus(string bonusName);
    }
}
