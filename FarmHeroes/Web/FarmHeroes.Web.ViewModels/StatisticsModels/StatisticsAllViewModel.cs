namespace FarmHeroes.Web.ViewModels.StatisticsModels
{
    using System;

    public class StatisticsAllViewModel
    {
        public string Name { get; set; }

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
    }
}
