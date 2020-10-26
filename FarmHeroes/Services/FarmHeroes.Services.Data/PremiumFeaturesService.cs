namespace FarmHeroes.Services.Data
{
    using System.Threading.Tasks;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;

    public class PremiumFeaturesService : IPremiumFeaturesService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;

        public PremiumFeaturesService(
            FarmHeroesDbContext context,
            IHeroService heroService)
        {
            this.context = context;
            this.heroService = heroService;
        }

        public async Task<PremiumFeatures> GetPremiumFeatures(int id = 0)
        {
            if (id == 0)
            {
                id = (await this.heroService.GetHero()).PremiumFeaturesId;
            }

            PremiumFeatures premiumFeatures = await this.context.PremiumFeatures.FindAsync(id);

            return premiumFeatures;
        }

        public async Task<bool> CheckIfEnabled(string featureName, int id = 0)
        {
            PremiumFeatures premiumFeatures = await this.GetPremiumFeatures(id);

            bool isEnabled = (bool)typeof(PremiumFeatures).GetProperty(featureName).GetValue(premiumFeatures);

            return isEnabled;
        }

        public async Task ToggleFeature(string featureName, int id = 0)
        {
            PremiumFeatures premiumFeatures = await this.GetPremiumFeatures(id);

            bool isEnabled = (bool)typeof(PremiumFeatures).GetProperty(featureName).GetValue(premiumFeatures);
            typeof(PremiumFeatures).GetProperty(featureName).SetValue(premiumFeatures, !isEnabled);

            await this.context.SaveChangesAsync();
        }
    }
}
