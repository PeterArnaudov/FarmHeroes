namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor httpContext;

        public ShopService(FarmHeroesDbContext context, IMapper mapper, IInventoryService inventoryService, IResourcePouchService resourcePouchService, ILevelService levelService, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.inventoryService = inventoryService;
            this.resourcePouchService = resourcePouchService;
            this.levelService = levelService;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.httpContext = httpContext;
        }

        public async Task<T> GetShopViewModel<T>(EquipmentType type)
        {
            ShopEquipment[] shopEquipment = await this.context.ShopEquipments
                .Where(x => x.Type == type)
                .OrderBy(x => x.RequiredLevel)
                .ToArrayAsync();

            T viewModel = this.mapper.Map<T>(shopEquipment);

            return viewModel;
        }

        public async Task<T> GetAmuletShopViewModel<T>()
        {
            ShopAmulet[] shopAmulets = await this.context.ShopAmulets.ToArrayAsync();

            T viewModel = this.mapper.Map<T>(shopAmulets);

            return viewModel;
        }

        public async Task<string> Sell(int id)
        {
            ShopEquipment shopEquipment = await this.context.ShopEquipments.FindAsync(id);
            int heroLevel = await this.levelService.GetCurrentHeroLevel();

            this.CheckIfRequiredLevelIsMet(shopEquipment, heroLevel);

            await this.resourcePouchService.DecreaseGold(shopEquipment.Price);

            HeroEquipment heroEquipment = new HeroEquipment
            {
                Name = shopEquipment.Name,
                Type = shopEquipment.Type,
                RequiredLevel = shopEquipment.RequiredLevel,
                ImageUrl = shopEquipment.ImageUrl,
                Attack = shopEquipment.Attack,
                Defense = shopEquipment.Defense,
                Mastery = shopEquipment.Mastery,
            };

            await this.inventoryService.InsertEquipment(heroEquipment);

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You bought {heroEquipment.Name}.");

            return heroEquipment.Type.ToString();
        }

        public async Task SellAmulet(int id)
        {
            ShopAmulet shopAmulet = await this.context.ShopAmulets.FindAsync(id);

            await this.resourcePouchService.DecreaseCrystals(shopAmulet.InitialPrice);

            HeroAmulet heroAmulet = new HeroAmulet
            {
                Name = shopAmulet.Name,
                Description = shopAmulet.Description,
                ImageUrl = shopAmulet.ImageUrl,
                InitialPrice = shopAmulet.InitialPrice,
                InitialBonus = shopAmulet.InitialBonus,
                Bonus = shopAmulet.InitialBonus,
            };

            await this.inventoryService.InsertAmulet(heroAmulet);

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You bought {heroAmulet.Name}.");
        }

        private void CheckIfRequiredLevelIsMet(ShopEquipment shopEquipment, int heroLevel)
        {
            if (heroLevel < shopEquipment.RequiredLevel)
            {
                throw new FarmHeroesException(
                    ShopExceptionMessages.RequiredLevelNotMetMessage,
                    ShopExceptionMessages.RequiredLevelNotMetInstruction,
                    string.Format(Redirects.ShopRedirect, shopEquipment.Type));
            }
        }
    }
}
