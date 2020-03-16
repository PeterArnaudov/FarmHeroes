namespace FarmHeroes.Data.Seeding
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class BonusesSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ShopBonuses.Any())
            {
                return;
            }

            ShopBonus[] bonuses = new ShopBonus[]
            {
                new ShopBonus() { Name = "Gold Safe", Description = "In case of a loss, the enemy will only steal half of what he would do usually.", InitialBonus = 0.5, CrystalsPrice = 250, ImageUrl = "/images/village/hut/gold-safe.jpg", AttainableFrom = "Hut", Days = 14, IsPermanent = false, Type = BonusType.Other },
                new ShopBonus() { Name = "Gold Totem", Description = "Increases your characteristics by 30% during fights.", InitialBonus = 0.3, CrystalsPrice = 350, ImageUrl = "/images/village/hut/gold-totem.jpg", AttainableFrom = "Hut", Days = 14, IsPermanent = false, Type = BonusType.Characteristics },
            };

            await dbContext.ShopBonuses.AddRangeAsync(bonuses);
        }
    }
}
