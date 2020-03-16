namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    using FarmHeroes.Data.Models.Enums;

    public class HeroBonus
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Level { get; set; } = 1;

        public double Bonus { get; set; }

        public BonusType Type { get; set; }

        public DateTime ActiveUntil { get; set; }

        public int HeroId { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
