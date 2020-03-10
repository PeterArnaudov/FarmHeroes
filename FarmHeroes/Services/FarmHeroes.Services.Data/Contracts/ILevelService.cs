namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.LevelModels;
    using System;
    using System.Threading.Tasks;

    public interface ILevelService
    {
        Task<int> GetCurrentHeroLevel();

        Task GiveCurrentHeroExperience(int experience);

        Task GiveHeroExperienceById(int id, int experience);

        Task LevelUpCurrentHero();

        Task LevelUpHeroById(int id);

        Task UpdateLevel(LevelModifyInputModel inputModel);
    }
}
