namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.Enums;
    using System;
    using System.Threading.Tasks;

    public interface IShopService
    {
        Task<T> GetShopViewModel<T>(EquipmentType type);

        Task<T> GetAmuletShopViewModel<T>();

        Task<string> Sell(int id);

        Task SellAmulet(int id);
    }
}
