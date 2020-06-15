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

        Task IncreaseResource(string resource, int amount, int id = 0);

        Task DecreaseResource(string resourceName, int amount, int id = 0);

        Task UpdateResourcePouch(ResourcePouchModifyInputModel inputModel);

        Task GivePassiveIncome();
    }
}
