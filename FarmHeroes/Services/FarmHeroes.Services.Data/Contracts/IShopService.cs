namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.Enums;
    using System;
    using System.Threading.Tasks;

    public interface IShopService
    {
        Task<T> GetShopViewModel<T>(EquipmentType type);

        Task<string> Sell(int id);
    }
}
