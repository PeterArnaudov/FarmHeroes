namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IMineService
    {
        Task InitiateDig();

        Task<int> Collect();

        Task CancelDig();
    }
}
