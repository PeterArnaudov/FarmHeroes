namespace FarmHeroes.Data.Seeding.EquipmentSeeders
{
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class HelmetsSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ShopHelmets.Any())
            {
                ShopHelmet[] shopHelmets = new ShopHelmet[]
                {
                    new ShopHelmet() { Name = "Mask", Price = 420, RequiredLevel = 3, Defense = 7, ImageUrl = "https://i.ibb.co/9wCVft4/helmet-1.jpg" },
                    new ShopHelmet() { Name = "Trough", Price = 1333, RequiredLevel = 5, Defense = 11, ImageUrl = "https://i.ibb.co/5sCWr6C/helmet-2.jpg" },
                    new ShopHelmet() { Name = "Strainer", Price = 4155, RequiredLevel = 7, Defense = 18, ImageUrl = "https://i.ibb.co/zfKjj1s/helmet-3.jpg" },
                    new ShopHelmet() { Name = "Metal Trough", Price = 5252, RequiredLevel = 9, Defense = 25, ImageUrl = "https://i.ibb.co/s9T4Y6q/helmet-4.jpg" },
                    new ShopHelmet() { Name = "Fancy Mask", Price = 5750, RequiredLevel = 10, Defense = 25, Attack = 5, ImageUrl = "https://i.ibb.co/X8Gv72F/helmet-5.jpg" },
                    new ShopHelmet() { Name = "Enforcer", Price = 6666, RequiredLevel = 12, Attack = 25, ImageUrl = "https://i.ibb.co/BNpPVD9/helmet-6.jpg" },
                    new ShopHelmet() { Name = "Rush", Price = 8400, RequiredLevel = 15, Defense = 25, Mastery = 15, ImageUrl = "https://i.ibb.co/mHDPcXz/helmet-7.jpg" },
                    new ShopHelmet() { Name = "Fuzzy", Price = 13480, RequiredLevel = 17, Defense = 32, Attack = 12, ImageUrl = "https://i.ibb.co/nDkn3Rh/helmet-8.jpg" },
                    new ShopHelmet() { Name = "Crimson Knight", Price = 16200, RequiredLevel = 19, Defense = 37, Attack = 14, ImageUrl = "https://i.ibb.co/B65jxMM/helmet-9.jpg" },
                    new ShopHelmet() { Name = "Cyclopus", Price = 21475, RequiredLevel = 22, Defense = 46, Attack = 21, ImageUrl = "https://i.ibb.co/qMTCbYj/helmet-10.jpg" },
                    new ShopHelmet() { Name = "Heavy Metal", Price = 57810, RequiredLevel = 25, Defense = 65, ImageUrl = "https://i.ibb.co/2N3y4sW/helmet-11.jpg" },
                    new ShopHelmet() { Name = "Feather", Price = 87890, RequiredLevel = 29, Defense = 15, Attack = 24, Mastery = 33, ImageUrl = "https://i.ibb.co/304Y2mK/helmet-13.jpg" },
                    new ShopHelmet() { Name = "Punisher", Price = 114880, RequiredLevel = 31, Defense = 40, Attack = 40, ImageUrl = "https://i.ibb.co/CBJYKqn/helmet-14.jpg" },
                    new ShopHelmet() { Name = "Monster Skull", Price = 147577, RequiredLevel = 34, Defense = 33, Attack = 33, Mastery = 33, ImageUrl = "https://i.ibb.co/86n9GL5/helmet-15.jpg" },
                };

                await dbContext.ShopHelmets.AddRangeAsync(shopHelmets);
            }
        }
    }
}
