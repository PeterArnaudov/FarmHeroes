namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;

    public interface ICharacteristicsService
    {
        Task<Characteristics> GetHeroCharacteristicsByIdAsync(int id);

        Task<Characteristics> GetCurrentHeroCharacteristicsAsync();

        Task<TViewModel> GetCurrentHeroCharacteristicsViewModelAsync<TViewModel>();

        Task<int> IncreaseAttack();

        Task<int> IncreaseDefense();

        Task<int> IncreaseMass();

        Task<int> IncreaseMastery();

        Task UpdateCharacteristics(CharacteristicsModifyInputModel inputModel);
    }
}
