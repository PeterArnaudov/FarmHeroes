namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.AmuletBagModels;
    using System;
    using System.Threading.Tasks;

    public interface IAmuletBagService
    {
        Task<AmuletBagViewModel> GetAmuletBagViewModel();

        Task ExtendRent();

        Task Purchase();

        Task Set(AmuletBagViewModel inputModel);

        Task EquipAmulet(string action);
    }
}
