namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ISmithService
    {
        Task Upgrade(int id);

        Task UpgradeAmulet(int id);
    }
}
