namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
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

        public async Task<string> Sell(int id)
        {
            ShopEquipment shopEquipment = await this.context.ShopEquipments.FindAsync(id);
            int heroLevel = await this.levelService.GetCurrentHeroLevel();

            if (heroLevel < shopEquipment.RequiredLevel)
            {
                throw new FarmHeroesException(
                    "You are lower level than the required one.",
                    "Level up before trying to buy yourself such mighty equipment.",
                    "/Shop/Helmets");
            }

            await this.resourcePouchService.DecreaseCurrentHeroGold(shopEquipment.Price);

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
    }
}
