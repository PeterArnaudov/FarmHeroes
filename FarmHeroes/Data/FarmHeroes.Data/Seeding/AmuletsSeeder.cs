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
                new ShopAmulet() { Name = "Crystal Digger", InitialBonus = 0.5, InitialPrice = 25, ImageUrl = "/images/equipment/amulets/amulet_1.jpg" },
                new ShopAmulet() { Name = "Undergrounder", InitialBonus = 0.125, InitialPrice = 50, ImageUrl = "/images/equipment/amulets/amulet_2.jpg" },
                new ShopAmulet() { Name = "Criticum", InitialBonus = 0.25, InitialPrice = 10, ImageUrl = "/images/equipment/amulets/amulet_3.jpg" },
                new ShopAmulet() { Name = "Fatty", InitialBonus = 0.125, InitialPrice = 15, ImageUrl = "/images/equipment/amulets/amulet_4.jpg" },
                new ShopAmulet() { Name = "Laborium", InitialBonus = 2.5, InitialPrice = 30, ImageUrl = "/images/equipment/amulets/amulet_5.jpg" },
                new ShopAmulet() { Name = "Speedster", InitialBonus = 0.5, InitialPrice = 5, ImageUrl = "/images/equipment/amulets/amulet_6.jpg" },
            };

            await dbContext.ShopAmulets.AddRangeAsync(amulets);
        }
    }
}
