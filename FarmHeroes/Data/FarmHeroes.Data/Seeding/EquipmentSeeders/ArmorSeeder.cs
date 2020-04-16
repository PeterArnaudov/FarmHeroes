namespace FarmHeroes.Data.Seeding.EquipmentSeeders
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ArmorSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ShopEquipments.Where(x => x.Type == EquipmentType.Armor).Any())
            {
                ShopEquipment[] shopArmor = new ShopEquipment[]
                {
                    new ShopEquipment() { Name = "Chafty", Type = EquipmentType.Armor, Price = 230, RequiredLevel = 2, Mastery = 3, Defense = 1, ImageUrl = "/images/equipment/armors/armor_1.jpg" },
                    new ShopEquipment() { Name = "Scrapster", Type = EquipmentType.Armor, Price = 846, RequiredLevel = 3, Mastery = 5, Attack = 2, ImageUrl = "/images/equipment/armors/armor_2.jpg" },
                    new ShopEquipment() { Name = "Cask", Type = EquipmentType.Armor, Price = 5489, RequiredLevel = 5, Defense = 10, Attack = 8, ImageUrl = "/images/equipment/armors/armor_3.jpg" },
                    new ShopEquipment() { Name = "Holley", Type = EquipmentType.Armor, Price = 8426, RequiredLevel = 7, Defense = 10, Mastery = 15, ImageUrl = "/images/equipment/armors/armor_4.jpg" },
                    new ShopEquipment() { Name = "Metzo", Type = EquipmentType.Armor, Price = 10875, RequiredLevel = 9, Attack = 15, Mastery = 15, ImageUrl = "/images/equipment/armors/armor_5.jpg" },
                    new ShopEquipment() { Name = "Modern Cask", Type = EquipmentType.Armor, Price = 14752, RequiredLevel = 11, Attack = 15, Mastery = 15, Defense = 15, ImageUrl = "/images/equipment/armors/armor_6.jpg" },
                    new ShopEquipment() { Name = "Rivvr", Type = EquipmentType.Armor, Price = 21589, RequiredLevel = 13, Defense = 48, ImageUrl = "/images/equipment/armors/armor_7.jpg" },
                    new ShopEquipment() { Name = "Abster", Type = EquipmentType.Armor, Price = 28555, RequiredLevel = 15, Attack = 43, ImageUrl = "/images/equipment/armors/armor_9.jpg" },
                    new ShopEquipment() { Name = "Crimа", Type = EquipmentType.Armor, Price = 49872, RequiredLevel = 20, Defense = 47, Attack = 11, Mastery = 38, ImageUrl = "/images/equipment/armors/armor_10.jpg" },
                    new ShopEquipment() { Name = "V", Type = EquipmentType.Armor, Price = 62357, RequiredLevel = 24, Defense = 57, Mastery = 74, ImageUrl = "/images/equipment/armors/armor_11.jpg" },
                    new ShopEquipment() { Name = "Angelo", Type = EquipmentType.Armor, Price = 83675, RequiredLevel = 27, Attack = 60, Defense = 120, ImageUrl = "/images/equipment/armors/armor_12.jpg" },
                    new ShopEquipment() { Name = "Sunn", Type = EquipmentType.Armor, Price = 109306, RequiredLevel = 29, Mastery = 168, ImageUrl = "/images/equipment/armors/armor_13.jpg" },
                    new ShopEquipment() { Name = "OM-88", Type = EquipmentType.Armor, Price = 148388, RequiredLevel = 31, Attack = 138, Defense = 33, ImageUrl = "/images/equipment/armors/armor_14.jpg" },
                    new ShopEquipment() { Name = "Saharum", Type = EquipmentType.Armor, Price = 178462, RequiredLevel = 34, Attack = 120, Defense = 60, Mastery = 30, ImageUrl = "/images/equipment/armors/armor_15.jpg" },
                    new ShopEquipment() { Name = "Skelto", Type = EquipmentType.Armor, Price = 217850, RequiredLevel = 36, Attack = 155, Defense = 155, ImageUrl = "/images/equipment/armors/armor_16.jpg" },
                    new ShopEquipment() { Name = "Aquavoz", Type = EquipmentType.Armor, Price = 258762, RequiredLevel = 38, Attack = 33, Defense = 57, Mastery = 194, ImageUrl = "/images/equipment/armors/armor_17.jpg" },
                };

                await dbContext.ShopEquipments.AddRangeAsync(shopArmor);
            }
        }
    }
}
