namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;

    public class LevelService : ILevelService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;

        public LevelService(FarmHeroesDbContext context, IHeroService heroService)
        {
            this.context = context;
            this.heroService = heroService;
        }

        public async Task<int> GetCurrentHeroLevel()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            return hero.Level.CurrentLevel;
        }

        public async Task GiveCurrentHeroExperience(int experience)
        {
            Hero hero = await this.heroService.GetCurrentHero();
            hero.Level.CurrentExperience += experience;

            if (hero.Level.CurrentExperience >= hero.Level.NeededExperience)
            {
                await this.LevelUpCurrentHero();
            }

            await this.context.SaveChangesAsync();
        }

        public async Task GiveHeroExperienceById(int id, int experience)
        {
            Hero hero = await this.heroService.GetHeroById(id);
            hero.Level.CurrentExperience += experience;

            if (hero.Level.CurrentExperience >= hero.Level.NeededExperience)
            {
                await this.LevelUpHeroById(id);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task LevelUpCurrentHero()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            int experienceLeft = hero.Level.CurrentExperience - hero.Level.NeededExperience;

            hero.Level.CurrentExperience = experienceLeft;
            hero.Level.NeededExperience = LevelFormulas.CalculateNextExperienceNeeded(hero.Level.NeededExperience);
            hero.Level.CurrentLevel++;

            await this.context.SaveChangesAsync();
        }

        public async Task LevelUpHeroById(int id)
        {
            Hero hero = await this.heroService.GetHeroById(id);
            int experienceLeft = hero.Level.CurrentExperience - hero.Level.NeededExperience;

            hero.Level.CurrentExperience = experienceLeft;
            hero.Level.NeededExperience = LevelFormulas.CalculateNextExperienceNeeded(hero.Level.NeededExperience);
            hero.Level.CurrentLevel++;

            await this.context.SaveChangesAsync();
        }
    }
}
