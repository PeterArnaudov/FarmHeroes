namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;
    using System.Threading.Tasks;

    public interface IEquipmentService
    {
        Task<EquippedSet> GetEquippedSet(int id = 0);

        Task Equip(int id);

        Task<AmuletViewModel> EquipAmulet(int id);
    }
}
