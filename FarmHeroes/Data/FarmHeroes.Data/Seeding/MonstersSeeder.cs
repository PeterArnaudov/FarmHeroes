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
                new Monster() { Name = "Zombie Miner", AvatarUrl = "https://i.ibb.co/4pV11MX/zombie-miner.jpg", Level = 1, StatPercentage = 50},
                new Monster() { Name = "Scarecrow", AvatarUrl = "https://i.ibb.co/JrXttjC/scarecrow.jpg", Level = 2, StatPercentage = 55},
                new Monster() { Name = "Fiery Chicken", AvatarUrl = "https://i.ibb.co/WDkLjDq/fiery-chicken.jpg", Level = 3, StatPercentage = 70},
                new Monster() { Name = "Green Slime", AvatarUrl = "https://i.ibb.co/yyr7X5D/green-slime.jpg", Level = 4, StatPercentage = 80},
                new Monster() { Name = "Hypnotizing Cat", AvatarUrl = "https://i.ibb.co/7WXq09G/hypnotizing-cat.jpg", Level = 5, StatPercentage = 90},
                new Monster() { Name = "Madshroom", AvatarUrl = "https://i.ibb.co/Rpnvxnd/madshroom.jpg", Level = 6, StatPercentage = 100},
                new Monster() { Name = "Panda-Assassin", AvatarUrl = "https://i.ibb.co/kmgkFpM/panda-assassin.jpg", Level = 7, StatPercentage = 110},
                new Monster() { Name = "Squeen", AvatarUrl = "https://i.ibb.co/1J8nbVF/squeen.jpg", Level = 8, StatPercentage = 125},
                new Monster() { Name = "Zirkus Firus", AvatarUrl = "https://i.ibb.co/DbJ9J10/zirkus-firus.jpg", Level = 9, StatPercentage = 150},
            };

            await dbContext.Monsters.AddRangeAsync(monsters);
        }
    }
}
