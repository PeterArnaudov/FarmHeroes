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
                new Monster() { Name = "Zombie Miner", AvatarUrl = "/images/monsters/zombie-miner.jpg", Level = 1, StatPercentage = 50 },
                new Monster() { Name = "Scarecrow", AvatarUrl = "/images/monsters/scarecrow.jpg", Level = 2, StatPercentage = 55},
                new Monster() { Name = "Fiery Chicken", AvatarUrl = "/images/monsters/fiery-chicken.jpg", Level = 3, StatPercentage = 70 },
                new Monster() { Name = "Green Slime", AvatarUrl = "/images/monsters/green-slime.jpg", Level = 4, StatPercentage = 80},
                new Monster() { Name = "Hypnotizing Cat", AvatarUrl = "/images/monsters/hypnotizing-cat.jpg", Level = 5, StatPercentage = 90},
                new Monster() { Name = "Madshroom", AvatarUrl = "/images/monsters/madshroom.jpg", Level = 6, StatPercentage = 100},
                new Monster() { Name = "Panda-Assassin", AvatarUrl = "/images/monsters/panda-assassin.jpg", Level = 7, StatPercentage = 110 },
                new Monster() { Name = "Squeen", AvatarUrl = "/images/monsters/squeen.jpg", Level = 8, StatPercentage = 125},
                new Monster() { Name = "Zirkus Firus", AvatarUrl = "/images/monsters/zirkus-firus.jpg", Level = 9, StatPercentage = 150 },
            };

            await dbContext.Monsters.AddRangeAsync(monsters);
        }
    }
}
