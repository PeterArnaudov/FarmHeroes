namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;

    public interface ICharacteristicsService
    {
        Task<Characteristics> GetCharacteristics(int id = 0);

        Task<TViewModel> GetCurrentHeroCharacteristicsViewModel<TViewModel>();

        Task<int> IncreaseAttack();

        Task<int> IncreaseDefense();

        Task<int> IncreaseMass();

        Task<int> IncreaseMastery();

        Task<int> IncreaseDexterity();

        Task UpdateCharacteristics(CharacteristicsModifyInputModel inputModel);
    }
}
