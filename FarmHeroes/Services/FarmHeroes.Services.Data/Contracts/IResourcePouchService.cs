namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;

    public interface IResourcePouchService
    {
        Task<ResourcePouch> GetResourcePouch(int id = 0);

        Task<TViewModel> GetCurrentHeroResourcesViewModel<TViewModel>();

        Task IncreaseGold(int gold, int id = 0);

        Task DecreaseGold(int gold, int id = 0);

        Task IncreaseCrystals(int crystals, int id = 0);

        Task DecreaseCrystals(int crystals, int id = 0);

        Task UpdateResourcePouch(ResourcePouchModifyInputModel inputModel);

        Task GivePassiveIncome();
    }
}
