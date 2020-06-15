namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class BonusService : IBonusService
    {
        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IMapper mapper;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;
        private readonly IHttpContextAccessor httpContext;

        public BonusService(IHeroService heroService, FarmHeroesDbContext context, IResourcePouchService resourcePouchService, IMapper mapper, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContext)
        {
            this.heroService = heroService;
            this.context = context;
            this.resourcePouchService = resourcePouchService;
            this.mapper = mapper;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
            this.httpContext = httpContext;
        }

        public async Task<T> GetBonusesViewModelForLocation<T>(string location)
        {
            ShopBonus[] bonuses = await this.context.ShopBonuses
                .Where(b => b.AttainableFrom == location)
                .ToArrayAsync();

            return this.mapper.Map<T>(bonuses);
        }

        public async Task ExtendBonus(string bonusName)
        {
            ShopBonus shopBonus = await this.GetShopBonus(bonusName);

            if (shopBonus == null || shopBonus.CrystalsPrice <= 0)
            {
                throw new FarmHeroesException(
                    "Such option doesn't exist",
                    "You probably tried to extend a non-existing bonus.",
                    "/Hero");
            }

            Hero hero = await this.heroService.GetHero();
            HeroBonus heroBonus = hero.Inventory.Bonuses.SingleOrDefault(b => b.Name == shopBonus.Name);

            if (heroBonus == null)
            {
                hero.Inventory.Bonuses.Add(this.GenerateHeroBonus(shopBonus));
            }
            else
            {
                heroBonus.ActiveUntil = shopBonus.IsPermanent ? DateTime.MaxValue
                    : heroBonus.ActiveUntil < DateTime.UtcNow ? DateTime.UtcNow.AddDays(shopBonus.Days)
                    : heroBonus.ActiveUntil.AddDays(shopBonus.Days);
            }

            await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, shopBonus.CrystalsPrice);

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You extended your rent on {shopBonus.Name}.");
        }

        private async Task<ShopBonus> GetShopBonus(string bonusName)
        {
            ShopBonus shopBonus = await this.context.ShopBonuses.SingleOrDefaultAsync(b => b.Name == bonusName);

            return shopBonus;
        }

        private HeroBonus GenerateHeroBonus(ShopBonus shopBonus)
        {
            return new HeroBonus
            {
                Name = shopBonus.Name,
                ImageUrl = shopBonus.ImageUrl,
                Description = shopBonus.Description,
                Bonus = shopBonus.InitialBonus,
                Type = shopBonus.Type,
                ActiveUntil = shopBonus.IsPermanent ? DateTime.MaxValue : DateTime.UtcNow.AddDays(shopBonus.Days),
            };
        }
    }
}
