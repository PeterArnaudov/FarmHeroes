﻿namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Statistics
    {
        public int Id { get; set; }

        public long TotalFights { get; set; }

        public long Wins { get; set; }

        public long Losses { get; set; }

        public long MaximalGoldSteal { get; set; }

        public long TotalGoldStolen { get; set; }

        public long TotalCrystalsStolen { get; set; }

        public long EarnedOnFarm { get; set; }

        public long EarnedOnPatrol { get; set; }

        public long EarnedInMines { get; set; }

        public long MonstersDefeated { get; set; }

        public long EarnedFromMonsters { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
