namespace FarmHeroes.Data.Seeding
{
    using FarmHeroes.Data.Models.MonsterModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    internal class MonstersSeeder : ISeeder
    {
        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Monsters.Any())
            {
                return;
            }

            Monster[] monsters = new Monster[]
            {
                new Monster() { Name = "Zombie Miner", Level = 1, StatPercentage = 50},
                new Monster() { Name = "Scarecrow", Level = 2, StatPercentage = 55},
                new Monster() { Name = "Fiery Chicken", Level = 3, StatPercentage = 70},
                new Monster() { Name = "Green Slime", Level = 4, StatPercentage = 80},
                new Monster() { Name = "Hypnotizing Cat", Level = 5, StatPercentage = 90},
                new Monster() { Name = "Madshroom", Level = 6, StatPercentage = 100},
                new Monster() { Name = "Panda-Assassin", Level = 7, StatPercentage = 110},
                new Monster() { Name = "Squeen", Level = 8, StatPercentage = 125},
                new Monster() { Name = "Zirkus Firus", Level = 9, StatPercentage = 150},
            };

            await dbContext.Monsters.AddRangeAsync(monsters);
        }
    }
}
