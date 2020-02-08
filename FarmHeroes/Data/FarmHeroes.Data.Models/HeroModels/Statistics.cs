namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Statistics
    {
        public int Id { get; set; }

        public int TotalFights { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int MaximalGoldSteal { get; set; }

        public int TotalGoldStolen { get; set; }

        public int TotalCrystalsStolen { get; set; }

        public int EarnedOnFarm { get; set; }

        public int EarnedOnPatrol { get; set; }

        public int EarnedInMines { get; set; }

        public int MonstersDefeated { get; set; }

        public int EarnedFromMonsters { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
