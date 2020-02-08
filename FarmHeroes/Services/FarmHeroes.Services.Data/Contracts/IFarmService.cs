namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IFarmService
    {
        Task StartWork();

        Task<int> Collect();

        Task CancelWork();
    }
}
