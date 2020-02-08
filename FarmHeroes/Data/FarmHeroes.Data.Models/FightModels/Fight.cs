namespace FarmHeroes.Data.Models.FightModels
{
    using System.Collections.Generic;

    using FarmHeroes.Data.Models.MappingModels;

    public class Fight
    {
        public int Id { get; set; }

        public virtual List<HeroFight> HeroFights { get; set; } = new List<HeroFight>();

        public long AttackerDamageDealt { get; set; }

        public long DefenderDamageDealt { get; set; }

        public long AttackerHealthLeft { get; set; }

        public long DefenderHealthLeft { get; set; }

        public string WinnerName { get; set; }

        public int HitCollectionId { get; set; }

        public virtual HitCollection HitCollection { get; set; }

        public int GoldStolen { get; set; }

        public int ExperienceGained { get; set; }
    }
}
