namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;

    public interface IResourcePouchService
    {
        public Task<ResourcePouch> GetHeroResourcesById(int id);

        Task<ResourcePouch> GetCurrentHeroResources();

        Task<TViewModel> GetCurrentHeroResourcesViewModel<TViewModel>();

        Task IncreaseGold(int id, int gold);

        Task IncreaseCurrentHeroGold(int gold);

        Task DecreaseGold(int id, int gold);

        Task DecreaseCurrentHeroGold(int gold);

        Task IncreaseCrystals(int id, int crystals);

        Task IncreaseCurrentHeroCrystals(int crystals);

        Task DecreaseCrystals(int id, int crystals);

        Task DecreaseCurrentHeroCrystals(int crystals);

        Task UpdateResourcePouch(ResourcePouchModifyInputModel inputModel);

        Task GivePassiveIncome();
    }
}
