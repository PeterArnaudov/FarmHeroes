namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;

    public interface ICharacteristicsService
    {
        Task<Characteristics> GetHeroCharacteristicsByIdAsync(int id);

        Task<Characteristics> GetCurrentHeroCharacteristicsAsync();

        Task<TViewModel> GetCurrentHeroCharacteristicsViewModelAsync<TViewModel>();

        Task IncreaseAttack();

        Task IncreaseDefense();

        Task IncreaseMass();

        Task IncreaseMastery();
    }
}
