namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System;
    using System.Threading.Tasks;

    public class SmithService : ISmithService
    {
        private const int HeroEquipmentMaximumLevel = 25;
        private const int HeroAmuletMaximumLevel = 100;

        private readonly IInventoryService inventoryService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly FarmHeroesDbContext context;
        private readonly IHttpContextAccessor httpContext;
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;

        public SmithService(IInventoryService inventoryService, IResourcePouchService resourcePouchService, FarmHeroesDbContext context, IHttpContextAccessor httpContext, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            this.inventoryService = inventoryService;
            this.resourcePouchService = resourcePouchService;
            this.context = context;
            this.httpContext = httpContext;
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public async Task Upgrade(int id)
        {
            Inventory inventory = await this.inventoryService.GetCurrentHeroInventory();
            HeroEquipment heroEquipment = inventory.Items.Find(i => i.Id == id);

            this.CheckIfItemIsOwnedByHero(heroEquipment);
            this.CheckIfItemIsFullyUpgraded(heroEquipment, HeroEquipmentMaximumLevel);

            int cost = SmithFormulas.CalculateEquipmentUpgradeCost(heroEquipment);
            await this.resourcePouchService.DecreaseCurrentHeroCrystals(cost);

            heroEquipment.Level += 5;

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                    .GetTempData(this.httpContext.HttpContext)
                    .Add("Alert", $"You upgraded {heroEquipment.Name}.");
        }

        public async Task UpgradeAmulet(int id)
        {
            Inventory inventory = await this.inventoryService.GetCurrentHeroInventory();
            HeroAmulet heroAmulet = inventory.Amulets.Find(i => i.Id == id);

            this.CheckIfItemIsOwnedByHero(heroAmulet);
            this.CheckIfItemIsFullyUpgraded(heroAmulet, HeroAmuletMaximumLevel);

            int cost = SmithFormulas.CalculateAmuletUpgradeCost(heroAmulet);
            await this.resourcePouchService.DecreaseCurrentHeroCrystals(cost);

            heroAmulet.Bonus += heroAmulet.Level == HeroAmuletMaximumLevel - 1 ? heroAmulet.InitialBonus * 101 : heroAmulet.InitialBonus;
            heroAmulet.Level++;

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                    .GetTempData(this.httpContext.HttpContext)
                    .Add("Alert", $"You upgraded {heroAmulet.Name}.");
        }

        private void CheckIfItemIsOwnedByHero(object item)
        {
            if (item == null)
            {
                throw new FarmHeroesException(
                    SmithExceptionMessages.ItemNotOwnedMessage,
                    SmithExceptionMessages.ItemNotOwnedInstruction,
                    Redirects.SmithRedirect);
            }
        }

        private void CheckIfItemIsFullyUpgraded(object item, int maxLevel)
        {
            if ((int)item.GetType().GetProperty("Level").GetValue(item) == maxLevel)
            {
                throw new FarmHeroesException(
                    SmithExceptionMessages.ItemFullyUpgradedMessage,
                    SmithExceptionMessages.ItemFullyUpgradedInstruction,
                    Redirects.SmithRedirect);
            }
        }
    }
}
