namespace FarmHeroes.Data.Seeding.EquipmentSeeders
{
    using FarmHeroes.Data.Models.ShopModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class WeaponsSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ShopWeapons.Any())
            {
                ShopWeapon[] shopWeapons = new ShopWeapon[]
                {
                    new ShopWeapon() { Name = "Rolling-pin", Price = 45, RequiredLevel = 1, Attack = 2, ImageUrl = "https://i.ibb.co/9y4j0hQ/weapon1.jpg" },
                    new ShopWeapon() { Name = "Ladle", Price = 80, RequiredLevel = 3, Attack = 3, ImageUrl = "https://i.ibb.co/Jd56zPx/weapon2.jpg" },
                    new ShopWeapon() { Name = "Enormous Handlebar", Price = 150, RequiredLevel = 4, Attack = 4, ImageUrl = "https://i.ibb.co/JdQrsKK/weapon3.jpg" },
                    new ShopWeapon() { Name = "Big Fork", Price = 240, RequiredLevel = 5, Attack = 6, ImageUrl = "https://i.ibb.co/rFx6S9n/weapon4.jpg" },
                    new ShopWeapon() { Name = "Nail Hammer", Price = 470, RequiredLevel = 6, Attack = 8, ImageUrl = "https://i.ibb.co/c6szTnN/weapon5.jpg" },
                    new ShopWeapon() { Name = "Wooden Sword", Price = 750, RequiredLevel = 7, Attack = 10, Mastery = 3, ImageUrl = "https://i.ibb.co/cyJ6496/weapon6.jpg" },
                    new ShopWeapon() { Name = "Farm Trident", Price = 1050, RequiredLevel = 8, Attack = 10, Mastery = 6, ImageUrl = "https://i.ibb.co/NWjVt0r/weapon7.jpg" },
                    new ShopWeapon() { Name = "Saber", Price = 1680, RequiredLevel = 9, Attack = 15, ImageUrl = "https://i.ibb.co/4fsJBYD/weapon8.jpg" },
                    new ShopWeapon() { Name = "Bat", Price = 2675, RequiredLevel = 10, Attack = 21, ImageUrl = "https://i.ibb.co/mcQKsXF/weapon9.jpg" },
                    new ShopWeapon() { Name = "Knife", Price = 3200, RequiredLevel = 12, Attack = 10, Mastery = 21, ImageUrl = "https://i.ibb.co/37tBQ6g/weapon10.jpg" },
                    new ShopWeapon() { Name = "Scythe", Price = 4716, RequiredLevel = 14, Attack = 27, Mastery = 10, ImageUrl = "https://i.ibb.co/B3XksYn/weapon11.jpg" },
                    new ShopWeapon() { Name = "Sickle", Price = 6666, RequiredLevel = 16, Attack = 16, Mastery = 34, ImageUrl = "https://i.ibb.co/C104zrR/weapon12.jpg" },
                };

                await dbContext.ShopWeapons.AddRangeAsync(shopWeapons);
            }
        }
    }
}
