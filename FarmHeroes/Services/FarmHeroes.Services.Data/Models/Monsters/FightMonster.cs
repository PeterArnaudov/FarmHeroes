namespace FarmHeroes.Services.Data.Models.Monsters
{
    using FarmHeroes.Data.Models.HeroModels;

    public class FightMonster
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public Characteristics Characteristics { get; set; }

        public int Health { get; set; }
    }
}
