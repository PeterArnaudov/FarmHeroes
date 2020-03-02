namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Threading.Tasks;

    public interface IInventoryService
    {
        Task<T> GetCurrentHeroInventoryViewModel<T>();

        Task Upgrade();

        Task Trash(int id);

        Task InsertEquipment(HeroEquipment heroEquipment);
    }
}
