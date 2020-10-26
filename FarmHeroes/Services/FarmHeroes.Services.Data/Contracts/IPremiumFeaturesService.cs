namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;

    public interface IPremiumFeaturesService
    {
        Task<PremiumFeatures> GetPremiumFeatures(int id = 0);

        Task<bool> CheckIfEnabled(string featureName, int id = 0);

        Task ToggleFeature(string featureName, int id = 0);
    }
}
