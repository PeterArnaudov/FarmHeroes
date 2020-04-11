namespace FarmHeroes.Data.Seeding.EquipmentSeeders
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShieldsSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ShopEquipments.Where(x => x.Type == EquipmentType.Shield).Any())
            {
                ShopEquipment[] shopShields = new ShopEquipment[]
               {
                    new ShopEquipment() { Name = "Wooden Shield", Type = EquipmentType.Shield, Price = 545, RequiredLevel = 2, Defense = 5, ImageUrl = "/images/equipment/shields/shield_1.jpg" },
                    new ShopEquipment() { Name = "Fluffster", Type = EquipmentType.Shield, Price = 2678, RequiredLevel = 5, Attack = 3, Defense = 10, ImageUrl = "/images/equipment/shields/shield_2.jpg" },
                    new ShopEquipment() { Name = "Spiky", Type = EquipmentType.Shield, Price = 4512, RequiredLevel = 7, Attack = 10, Defense = 16, ImageUrl = "/images/equipment/shields/shield_3.jpg" },
                    new ShopEquipment() { Name = "Antique", Type = EquipmentType.Shield, Price = 12365, RequiredLevel = 13, Defense = 20, Mastery = 12, ImageUrl = "/images/equipment/shields/shield_4.jpg" },
                    new ShopEquipment() { Name = "Gramadan", Type = EquipmentType.Shield, Price = 34575, RequiredLevel = 18, Defense = 48, Mastery = -15, ImageUrl = "/images/equipment/shields/shield_5.jpg" },
                    new ShopEquipment() { Name = "Fabricco", Type = EquipmentType.Shield, Price = 78054, RequiredLevel = 22, Defense = 33, Mastery = 27, ImageUrl = "/images/equipment/shields/shield_6.jpg" },
                    new ShopEquipment() { Name = "Berr", Type = EquipmentType.Shield, Price = 167985, RequiredLevel = 26, Attack = 10, Defense = 44, Mastery = 8, ImageUrl = "/images/equipment/shields/shield_7.jpg" },
                    new ShopEquipment() { Name = "BonFish", Type = EquipmentType.Shield, Price = 338833, RequiredLevel = 30, Attack = 20, Defense = 29, Mastery = 24, ImageUrl = "/images/equipment/shields/shield_8.jpg" },
                    new ShopEquipment() { Name = "Spikster", Type = EquipmentType.Shield, Price = 699785, RequiredLevel = 35, Attack = 54, Defense = 32, Mastery = -20, ImageUrl = "/images/equipment/shields/shield_9.jpg" },
                    new ShopEquipment() { Name = "Druid", Type = EquipmentType.Shield, Price = 845788, RequiredLevel = 38, Defense = 72, Mastery = 28, ImageUrl = "/images/equipment/shields/shield_10.jpg" },
                    new ShopEquipment() { Name = "Smiley", Type = EquipmentType.Shield, Price = 1145789, RequiredLevel = 42, Attack = 30, Defense = 60, Mastery = 30, ImageUrl = "/images/equipment/shields/shield_11.jpg" },
                    new ShopEquipment() { Name = "Lunari", Type = EquipmentType.Shield, Price = 1789242, RequiredLevel = 46, Attack = 46, Defense = 42, Mastery = 50, ImageUrl = "/images/equipment/shields/shield_12.jpg" },
                    new ShopEquipment() { Name = "Rogn", Type = EquipmentType.Shield, Price = 2589752, RequiredLevel = 50, Attack = 88, Defense = 99, Mastery = -30, ImageUrl = "/images/equipment/shields/shield_13.jpg" },
                    new ShopEquipment() { Name = "X", Type = EquipmentType.Shield, Price = 2899452, RequiredLevel = 52, Attack = 22, Defense = 68, Mastery = 112, ImageUrl = "/images/equipment/shields/shield_14.jpg" },
                    new ShopEquipment() { Name = "Monstro", Type = EquipmentType.Shield, Price = 3386424, RequiredLevel = 54, Attack = 51, Defense = 133, Mastery = 18, ImageUrl = "/images/equipment/shields/shield_15.jpg" },
                    new ShopEquipment() { Name = "Cancroid", Type = EquipmentType.Shield, Price = 3755378, RequiredLevel = 57, Attack = 112, Defense = 138, ImageUrl = "/images/equipment/shields/shield_16.jpg" },
               };

                await dbContext.ShopEquipments.AddRangeAsync(shopShields);
            }
        }
    }
}
