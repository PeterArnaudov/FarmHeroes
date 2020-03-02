namespace FarmHeroes.Data.Seeding.EquipmentSeeders
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class HelmetsSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ShopEquipments.Where(x => x.Type == EquipmentType.Helmet).Any())
            {
                ShopEquipment[] shopHelmets = new ShopEquipment[]
                {
                    new ShopEquipment() { Name = "Mask", Type = EquipmentType.Helmet, Price = 420, RequiredLevel = 3, Defense = 7, ImageUrl = "https://i.ibb.co/9wCVft4/helmet-1.jpg" },
                    new ShopEquipment() { Name = "Trough", Type = EquipmentType.Helmet, Price = 1333, RequiredLevel = 5, Defense = 11, ImageUrl = "https://i.ibb.co/5sCWr6C/helmet-2.jpg" },
                    new ShopEquipment() { Name = "Strainer", Type = EquipmentType.Helmet, Price = 4155, RequiredLevel = 7, Defense = 18, ImageUrl = "https://i.ibb.co/zfKjj1s/helmet-3.jpg" },
                    new ShopEquipment() { Name = "Metal Trough", Type = EquipmentType.Helmet, Price = 5252, RequiredLevel = 9, Defense = 25, ImageUrl = "https://i.ibb.co/s9T4Y6q/helmet-4.jpg" },
                    new ShopEquipment() { Name = "Fancy Mask", Type = EquipmentType.Helmet, Price = 5750, RequiredLevel = 10, Defense = 25, Attack = 5, ImageUrl = "https://i.ibb.co/X8Gv72F/helmet-5.jpg" },
                    new ShopEquipment() { Name = "Enforcer", Type = EquipmentType.Helmet, Price = 6666, RequiredLevel = 12, Attack = 25, ImageUrl = "https://i.ibb.co/BNpPVD9/helmet-6.jpg" },
                    new ShopEquipment() { Name = "Rush", Type = EquipmentType.Helmet, Price = 8400, RequiredLevel = 15, Defense = 25, Mastery = 15, ImageUrl = "https://i.ibb.co/mHDPcXz/helmet-7.jpg" },
                    new ShopEquipment() { Name = "Fuzzy", Type = EquipmentType.Helmet, Price = 13480, RequiredLevel = 17, Defense = 32, Attack = 12, ImageUrl = "https://i.ibb.co/nDkn3Rh/helmet-8.jpg" },
                    new ShopEquipment() { Name = "Crimson Knight", Type = EquipmentType.Helmet, Price = 16200, RequiredLevel = 19, Defense = 37, Attack = 14, ImageUrl = "https://i.ibb.co/B65jxMM/helmet-9.jpg" },
                    new ShopEquipment() { Name = "Cyclopus", Type = EquipmentType.Helmet, Price = 21475, RequiredLevel = 22, Defense = 46, Attack = 21, ImageUrl = "https://i.ibb.co/qMTCbYj/helmet-10.jpg" },
                    new ShopEquipment() { Name = "Heavy Metal", Type = EquipmentType.Helmet, Price = 57810, RequiredLevel = 25, Defense = 65, ImageUrl = "https://i.ibb.co/2N3y4sW/helmet-11.jpg" },
                    new ShopEquipment() { Name = "Feather", Type = EquipmentType.Helmet, Price = 87890, RequiredLevel = 29, Defense = 15, Attack = 24, Mastery = 33, ImageUrl = "https://i.ibb.co/304Y2mK/helmet-13.jpg" },
                    new ShopEquipment() { Name = "Punisher", Type = EquipmentType.Helmet, Price = 114880, RequiredLevel = 31, Defense = 40, Attack = 40, ImageUrl = "https://i.ibb.co/CBJYKqn/helmet-14.jpg" },
                    new ShopEquipment() { Name = "Monster Skull", Type = EquipmentType.Helmet, Price = 147577, RequiredLevel = 34, Defense = 33, Attack = 33, Mastery = 33, ImageUrl = "https://i.ibb.co/86n9GL5/helmet-15.jpg" },
                };

                await dbContext.ShopEquipments.AddRangeAsync(shopHelmets);
            }
        }
    }
}
