namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.HeroModels;
    using System.Threading.Tasks;

    public interface IHarbourService
    {
        Task<int> BuyFishingVessel(string vessel);

        Task<int> SetSail(int id = 0);

        Task<int> Collect(int id = 0);

        Task ManagerSetSail(int id);

        Task CollectAll();
    }
}
