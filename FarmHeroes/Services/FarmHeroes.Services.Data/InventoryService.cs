namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Common.Repositories;
    using FarmHeroes.Data.Models.HeroModels;

    public class InventoryService
    {
        private readonly FarmHeroesDbContext context;

        public InventoryService(FarmHeroesDbContext context)
        {
            this.context = context;
        }

        public async Task CreateInventory()
        {
            Inventory inventory = new Inventory();
            await this.context.Inventories.AddAsync(inventory);
        }
    }
}
