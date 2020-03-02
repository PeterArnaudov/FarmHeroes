namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IShopService
    {
        Task<T> GetShopViewModel<T>(string type);

        Task SellHelmet(int id);

        Task SellArmor(int id);

        Task SellWeapon(int id);

        Task SellShield(int id);
    }
}
