namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using System;
    using System.Threading.Tasks;

    public interface IMineService
    {
        Task<int> InitiateDig();

        Task<CollectedResourcesViewModel> Collect();
    }
}
