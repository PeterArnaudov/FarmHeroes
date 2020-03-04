namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using System;
    using System.Threading.Tasks;

    public interface IFarmService
    {
        Task<int> StartWork();

        Task<CollectedResourcesViewModel> Collect();
    }
}
