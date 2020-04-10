﻿namespace FarmHeroes.Data.Seeding
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
                new ShopAmulet() { Name = "Undergrounder", Description = "Increases your characteristics when fighting with monsters.", InitialBonus = 0.125, InitialPrice = 50, ImageUrl = "/images/equipment/amulets/amulet_2.jpg" },
                new ShopAmulet() { Name = "Criticum", Description = "Increases your chance to inflict a critical strike on your opponent.", InitialBonus = 0.25, InitialPrice = 10, ImageUrl = "/images/equipment/amulets/amulet_3.jpg" },
                new ShopAmulet() { Name = "Fatty", Description = "Increases the amount of blocked damage from your opponent's hits.", InitialBonus = 0.125, InitialPrice = 15, ImageUrl = "/images/equipment/amulets/amulet_4.jpg" },
                new ShopAmulet() { Name = "Laborium", Description = "Increases your farm salary.", InitialBonus = 2.5, InitialPrice = 30, ImageUrl = "/images/equipment/amulets/amulet_5.jpg" },
                new ShopAmulet() { Name = "Speedster", Description = "Gives you chance to finish your patrol for 10 seconds.", InitialBonus = 0.5, InitialPrice = 5, ImageUrl = "/images/equipment/amulets/amulet_6.jpg" },
            };

            await dbContext.ShopAmulets.AddRangeAsync(amulets);
        }
    }
}
