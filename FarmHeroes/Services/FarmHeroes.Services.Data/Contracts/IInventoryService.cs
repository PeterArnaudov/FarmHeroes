namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Threading.Tasks;

    public interface IInventoryService
    {
        Task<Inventory> GetCurrentHeroInventory();

        Task<T> GetCurrentHeroInventoryViewModel<T>();

        Task<T> GetCurrentHeroAmuletsViewModel<T>();

        Task Upgrade();

        Task Trash(int id);

        Task InsertEquipment(HeroEquipment heroEquipment);

        Task InsertAmulet(HeroAmulet heroAmulet);
    }
}
