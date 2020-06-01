namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.LevelModels;
    using System;
    using System.Threading.Tasks;

    public interface ILevelService
    {
        Task<int> GetCurrentHeroLevel();

        Task GiveHeroExperience(int experience, int id = 0);

        Task UpdateLevel(LevelModifyInputModel inputModel);
    }
}
