namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
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

            if (heroEquipment == null)
            {
                throw new FarmHeroesException(
                    "You cannot upgrade an item that isn't in your inventory.",
                    "Choose an item from your inventory.",
                    "/Smith");
            }
            else if (heroEquipment.Level == HeroEquipmentMaximumLevel)
            {
                throw new FarmHeroesException(
                    "This item is already upgraded to its maximum level.",
                    "Choose an item that isn't fully upgraded.",
                    "/Smith");
            }

            int cost = SmithFormulas.CalculateEquipmentUpgradeCost(heroEquipment);
            await this.resourcePouchService.DecreaseCurrentHeroCrystals(cost);

            heroEquipment.Level += 5;

            await this.context.SaveChangesAsync();

            this.tempDataDictionaryFactory
                    .GetTempData(this.httpContext.HttpContext)
                    .Add("Alert", $"You upgraded {heroEquipment.Name}.");
        }
    }
}
