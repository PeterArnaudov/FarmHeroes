namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
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
            Hero hero = await this.heroService.GetHero();

            return hero.Inventory;
        }

        public async Task<T> GetCurrentHeroInventoryViewModel<T>()
        {
            Hero hero = await this.heroService.GetHero();

            T viewModel = this.mapper.Map<T>(hero.Inventory);

            return viewModel;
        }

        public async Task InsertEquipment(HeroEquipment heroEquipment)
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            this.CheckIfInventoryHasFreeSpace(inventory, heroEquipment);

            inventory.Items.Add(heroEquipment);

            await this.context.SaveChangesAsync();
        }

        public async Task InsertAmulet(HeroAmulet heroAmulet)
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            inventory.Amulets.Add(heroAmulet);

            await this.context.SaveChangesAsync();
        }

        public async Task Upgrade()
        {
            Inventory inventory = await this.GetCurrentHeroInventory();

            this.CheckIfInventoryCanBeUpgraded(inventory);

            await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, InventoryFormulas.CalculateUpgradeCost(inventory.MaximumCapacity));

            inventory.MaximumCapacity++;

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You upgraded your inventory successfully.");
        }

        public async Task Trash(int id)
        {
            Hero hero = await this.heroService.GetHero();
            HeroEquipment itemToRemove = hero.Inventory.Items.Find(i => i.Id == id);
            HeroAmulet amuletToRemove = hero.Inventory.Amulets.Find(a => a.Id == id);

            if (itemToRemove != null)
            {
                this.context.HeroEquipments.Remove(itemToRemove);
            }
            else if (amuletToRemove != null)
            {
                this.context.HeroAmulets.Remove(amuletToRemove);

                typeof(AmuletBag)
                    .GetProperties()
                    .Where(x => x.Name.StartsWith("On") && (int)x.GetValue(hero.AmuletBag) == id)
                    .ToList()
                    .ForEach(x => x.SetValue(hero.AmuletBag, 0));
            }
            else
            {
                throw new FarmHeroesException(
                    InventoryExceptionMessages.ItemNotOwnedMessage,
                    InventoryExceptionMessages.ItemNotOwnedInstruction,
                    Redirects.InventoryRedirect);
            }

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                .GetTempData(this.httpContext.HttpContext)
                .Add("Alert", $"You trashed the desired item successfully.");
        }

        private void CheckIfInventoryHasFreeSpace(Inventory inventory, HeroEquipment heroEquipment)
        {
            if (inventory.Items.Count == inventory.MaximumCapacity)
            {
                throw new FarmHeroesException(
                    InventoryExceptionMessages.NotEnoughSpaceMessage,
                    InventoryExceptionMessages.NotEnoughSpaceInstruction,
                    string.Format(Redirects.ShopRedirect, heroEquipment.Type));
            }
        }

        private void CheckIfInventoryCanBeUpgraded(Inventory inventory)
        {
            if (inventory.MaximumCapacity == MaximumCapacityPossible)
            {
                throw new FarmHeroesException(
                    InventoryExceptionMessages.MaximumUpgradeReachedMessage,
                    InventoryExceptionMessages.MaximumUpgradeReachedInstruction,
                    Redirects.InventoryRedirect);
            }
        }
    }
}
