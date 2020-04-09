namespace FarmHeroes.Data.Seeding
{
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AmuletsSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ShopAmulets.Any())
            {
                return;
            }

            ShopAmulet[] amulets = new ShopAmulet[]
            {
                new ShopAmulet() { Name = "Crystal Digger", Description = "Gives you chance to double the crystals you collect from the mines.", InitialBonus = 0.5, InitialPrice = 25, ImageUrl = "/images/equipment/amulets/amulet_1.jpg" },
                new ShopAmulet() { Name = "Undergrounder", Description = "Increases your characteristics when fighting with monsters.", InitialBonus = 0.25, InitialPrice = 50, ImageUrl = "/images/equipment/amulets/amulet_2.jpg"},
            };

            await dbContext.ShopAmulets.AddRangeAsync(amulets);
        }
    }
}
