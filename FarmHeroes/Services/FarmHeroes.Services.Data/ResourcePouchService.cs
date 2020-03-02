namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;

    public class ResourcePouchService : IResourcePouchService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;
        private readonly IMapper mapper;

        public ResourcePouchService(FarmHeroesDbContext context, IHeroService heroService, IMapper mapper)
        {
            this.context = context;
            this.heroService = heroService;
            this.mapper = mapper;
        }

        public async Task<ResourcePouch> GetHeroResourcesById(int id)
        {
            ResourcePouch resources = await this.context.ResourcePouches.FindAsync(id);
            return resources;
        }

        public async Task<ResourcePouch> GetCurrentHeroResources()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            ResourcePouch resources = hero.ResourcePouch;

            return resources;
        }

        public async Task<TViewModel> GetCurrentHeroResourcesViewModel<TViewModel>()
        {
            ResourcePouch resourcePouch = await this.GetCurrentHeroResources();
            TViewModel viewModel = this.mapper.Map<TViewModel>(resourcePouch);

            return viewModel;
        }

        public async Task IncreaseGold(int id, int gold)
        {
            ResourcePouch resources = await this.context.ResourcePouches.FindAsync(id);
            resources.Gold += gold;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseCurrentHeroGold(int gold)
        {
            ResourcePouch resources = await this.GetCurrentHeroResources();
            resources.Gold += gold;

            await this.context.SaveChangesAsync();
        }

        public async Task DecreaseGold(int id, int gold)
        {
            ResourcePouch resources = await this.context.ResourcePouches.FindAsync(id);

            if (resources.Gold < gold)
            {
                throw new Exception("The hero doesn't have enough gold.");
            }

            resources.Gold -= gold;

            await this.context.SaveChangesAsync();
        }

        public async Task DecreaseCurrentHeroGold(int gold)
        {
            ResourcePouch resources = await this.GetCurrentHeroResources();

            if (resources.Gold < gold)
            {
                throw new FarmHeroesException(
                    "You don't have enough gold.",
                    "Go earn yourself some more gold and come back... or steal it. ;)",
                    "/Battlefield");
            }

            resources.Gold -= gold;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseCrystals(int id, int crystals)
        {
            ResourcePouch resources = await this.context.ResourcePouches.FindAsync(id);
            resources.Crystals += crystals;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseCurrentHeroCrystals(int crystals)
        {
            ResourcePouch resources = await this.GetCurrentHeroResources();
            resources.Crystals += crystals;

            await this.context.SaveChangesAsync();
        }

        public async Task DecreaseCrystals(int id, int crystals)
        {
            ResourcePouch resources = await this.context.ResourcePouches.FindAsync(id);

            if (resources.Crystals < crystals)
            {
                throw new Exception("The hero doesn't have enough crystals.");
            }

            resources.Crystals -= crystals;

            await this.context.SaveChangesAsync();
        }

        public async Task DecreaseCurrentHeroCrystals(int crystals)
        {
            ResourcePouch resources = await this.GetCurrentHeroResources();

            if (resources.Crystals < crystals)
            {
                throw new FarmHeroesException(
                    "You don't have enough crystals.",
                    "Go earn yourself some more crystals and come back.",
                    "/Mine");
            }

            resources.Crystals -= crystals;

            await this.context.SaveChangesAsync();
        }
    }
}
