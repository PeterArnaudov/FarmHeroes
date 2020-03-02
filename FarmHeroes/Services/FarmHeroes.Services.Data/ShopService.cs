namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShopService : IShopService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;
        private readonly IInventoryService inventoryService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly ILevelService levelService;

        public ShopService(FarmHeroesDbContext context, IMapper mapper, IInventoryService inventoryService, IResourcePouchService resourcePouchService, ILevelService levelService)
        {
            this.context = context;
            this.mapper = mapper;
            this.inventoryService = inventoryService;
            this.resourcePouchService = resourcePouchService;
            this.levelService = levelService;
        }

        public async Task<T> GetShopViewModel<T>(string type)
        {
            ShopEquipment[] shopEquipment = null;
            if (type == "Helmet")
            {
                shopEquipment = await this.context.ShopHelmets.ToArrayAsync();
            }
            else if (type == "Armor")
            {
                shopEquipment = await this.context.ShopArmors.ToArrayAsync();
            }
            else if (type == "Weapon")
            {
                shopEquipment = await this.context.ShopWeapons.ToArrayAsync();
            }
            else if (type == "Shield")
            {
                shopEquipment = await this.context.ShopShields.ToArrayAsync();
            }

            shopEquipment = shopEquipment.OrderBy(x => x.RequiredLevel).ToArray();

            T viewModel = this.mapper.Map<T>(shopEquipment);

            return viewModel;
        }

        public async Task SellHelmet(int id)
        {
            ShopHelmet shopHelmet = this.context.ShopHelmets.Find(id);
            int heroLevel = await this.levelService.GetCurrentHeroLevel();

            if (heroLevel < shopHelmet.RequiredLevel)
            {
                throw new FarmHeroesException(
                    "You are lower level than the required one.",
                    "Level up before trying to buy yourself such mighty equipment.",
                    "/Shop/Helmets");
            }

            await this.resourcePouchService.DecreaseCurrentHeroGold(shopHelmet.Price);

            HeroHelmet heroHelmet = new HeroHelmet
            {
                Name = shopHelmet.Name,
                RequiredLevel = shopHelmet.RequiredLevel,
                ImageUrl = shopHelmet.ImageUrl,
                Attack = shopHelmet.Attack,
                Defense = shopHelmet.Defense,
                Mastery = shopHelmet.Mastery,
            };

            await this.inventoryService.InsertEquipment(heroHelmet);
        }

        public async Task SellArmor(int id)
        {
            ShopArmor shopArmor = this.context.ShopArmors.Find(id);
            int heroLevel = await this.levelService.GetCurrentHeroLevel();

            if (heroLevel < shopArmor.RequiredLevel)
            {
                throw new FarmHeroesException(
                    "You are lower level than the required one.",
                    "Level up before trying to buy yourself such mighty equipment.",
                    "/Shop/Armor");
            }

            await this.resourcePouchService.DecreaseCurrentHeroGold(shopArmor.Price);

            HeroArmor heroArmor = new HeroArmor
            {
                Name = shopArmor.Name,
                RequiredLevel = shopArmor.RequiredLevel,
                ImageUrl = shopArmor.ImageUrl,
                Attack = shopArmor.Attack,
                Defense = shopArmor.Defense,
                Mastery = shopArmor.Mastery,
            };

            await this.inventoryService.InsertEquipment(heroArmor);
        }

        public async Task SellWeapon(int id)
        {
            ShopWeapon shopWeapon = this.context.ShopWeapons.Find(id);
            int heroLevel = await this.levelService.GetCurrentHeroLevel();

            if (heroLevel < shopWeapon.RequiredLevel)
            {
                throw new FarmHeroesException(
                    "You are lower level than the required one.",
                    "Level up before trying to buy yourself such mighty equipment.",
                    "/Shop/Weapons");
            }

            await this.resourcePouchService.DecreaseCurrentHeroGold(shopWeapon.Price);

            HeroWeapon heroWeapon = new HeroWeapon
            {
                Name = shopWeapon.Name,
                RequiredLevel = shopWeapon.RequiredLevel,
                ImageUrl = shopWeapon.ImageUrl,
                Attack = shopWeapon.Attack,
                Defense = shopWeapon.Defense,
                Mastery = shopWeapon.Mastery,
            };

            await this.inventoryService.InsertEquipment(heroWeapon);
        }

        public async Task SellShield(int id)
        {
            ShopShield shopShield = this.context.ShopShields.Find(id);
            int heroLevel = await this.levelService.GetCurrentHeroLevel();

            if (heroLevel < shopShield.RequiredLevel)
            {
                throw new FarmHeroesException(
                    "You are lower level than the required one.",
                    "Level up before trying to buy yourself such mighty equipment.",
                    "/Shop/Shields");
            }

            await this.resourcePouchService.DecreaseCurrentHeroGold(shopShield.Price);

            HeroShield heroShield = new HeroShield
            {
                Name = shopShield.Name,
                RequiredLevel = shopShield.RequiredLevel,
                ImageUrl = shopShield.ImageUrl,
                Attack = shopShield.Attack,
                Defense = shopShield.Defense,
                Mastery = shopShield.Mastery,
            };

            await this.inventoryService.InsertEquipment(heroShield);
        }
    }
}
