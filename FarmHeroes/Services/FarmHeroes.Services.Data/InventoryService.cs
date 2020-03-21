namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Common.Repositories;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class InventoryService : IInventoryService
    {
        private const int MaximumCapacityPossible = 20;

        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;

        public InventoryService(IHeroService heroService, IResourcePouchService resourcePouchService, FarmHeroesDbContext context, IMapper mapper, IHttpContextAccessor httpContext, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.context = context;
            this.mapper = mapper;
            this.httpContext = httpContext;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public async Task<Inventory> GetCurrentHeroInventory()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            return hero.Inventory;
        }

        public async Task<T> GetCurrentHeroInventoryViewModel<T>()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            T viewModel = this.mapper.Map<T>(hero.Inventory);

            return viewModel;
        }

        public async Task InsertEquipment(HeroEquipment heroEquipment)
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            if (inventory.Items.Count == inventory.MaximumCapacity)
            {
                throw new FarmHeroesException(
                    "You don't have enough space in your inventory.",
                    "Upgrade your inventory or free up some space by selling something you don't need.",
                    $"/Shop/{heroEquipment.Type.ToString()}");
            }

            inventory.Items.Add(heroEquipment);

            await this.context.SaveChangesAsync();
        }

        public async Task InsertAmulet(HeroAmulet heroAmulet)
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            inventory.Amulets.Add(heroAmulet);

            await this.context.SaveChangesAsync();
        }

        public async Task<T> GetCurrentHeroAmuletsViewModel<T>()
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            T viewModel = this.mapper.Map<T>(inventory);

            return viewModel;
        }

        public async Task Upgrade()
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            if (inventory.MaximumCapacity == MaximumCapacityPossible)
            {
                throw new FarmHeroesException(
                    "You've reached the maximum possible upgrade of the inventory.",
                    "You cannot upgrade futher.",
                    "/Inventory");
            }

            await this.resourcePouchService.DecreaseCurrentHeroCrystals(InventoryFormulas.CalculateUpgradeCost(inventory.MaximumCapacity));

            inventory.MaximumCapacity++;

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You upgraded your inventory successfully.");
        }

        public async Task Trash(int id)
        {
            Inventory inventory = await this.GetCurrentHeroInventory();
            HeroEquipment itemToRemove = inventory.Items.Find(i => i.Id == id);
            HeroAmulet amuletToRemove = inventory.Amulets.Find(a => a.Id == id);

            if (itemToRemove != null)
            {
                this.context.HeroEquipments.Remove(itemToRemove);
            }
            else if (amuletToRemove != null)
            {
                this.context.HeroAmulets.Remove(amuletToRemove);
            }
            else
            {
                throw new FarmHeroesException(
                    "You cannot trash an item that isn't in your inventory.",
                    "Choose an item from your inventory.",
                    "/Inventory");
            }

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You trashed the desired item successfully.");
        }
    }
}
