namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class EquipmentService : IEquipmentService
    {
        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;
        private readonly IHttpContextAccessor httpContext;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;

        public EquipmentService(IHeroService heroService, FarmHeroesDbContext context, IHttpContextAccessor httpContext, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            this.heroService = heroService;
            this.context = context;
            this.httpContext = httpContext;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public async Task<EquippedSet> GetCurrentHeroEquipedSet()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            return hero.EquippedSet;
        }

        public async Task<EquippedSet> GetEquippedSetById(int id)
        {
            EquippedSet equippedSet = await this.context.EquippedSets.FindAsync(id);

            return equippedSet;
        }

        public async Task Equip(int id)
        {
            Hero hero = await this.heroService.GetCurrentHero();
            HeroEquipment heroEquipment = await this.GetHeroEquipmentById(id);
            EquippedSet equippedSet = await this.GetCurrentHeroEquipedSet();

            if (hero.InventoryId != heroEquipment.InventoryId)
            {
                throw new FarmHeroesException(
                    "You cannot equip an item that isn't yours.",
                    "Go buy yourself items.",
                    "/Inventory");
            }

            if (equippedSet.Equipped.Any(x => x.Type == heroEquipment.Type))
            {
                equippedSet.Equipped.Remove(equippedSet.Equipped.Find(x => x.Type == heroEquipment.Type));
            }

            equippedSet.Equipped.Add(heroEquipment);

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You equipped {heroEquipment.Name}.");
        }

        public async Task EquipAmulet(int id)
        {
            Hero hero = await this.heroService.GetCurrentHero();
            HeroAmulet heroAmulet = await this.GetHeroAmuletById(id);
            EquippedSet equippedSet = await this.GetCurrentHeroEquipedSet();

            if (hero.InventoryId != heroAmulet.InventoryId)
            {
                throw new FarmHeroesException(
                    "You cannot equip an item that isn't yours.",
                    "Go buy yourself items.",
                    "/Inventory");
            }

            equippedSet.Amulet = heroAmulet;

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You equipped {heroAmulet.Name}.");
        }

        private async Task<HeroEquipment> GetHeroEquipmentById(int id)
        {
            HeroEquipment heroEquipment = await this.context.HeroEquipments.FindAsync(id);

            return heroEquipment;
        }

        private async Task<HeroAmulet> GetHeroAmuletById(int id)
        {
            HeroAmulet heroAmulet = await this.context.HeroAmulets.FindAsync(id);

            return heroAmulet;
        }
    }
}
