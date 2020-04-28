namespace FarmHeroes.Web.ViewModels.HeroModels
{
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;

    public class HeroViewModel
    {
        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public int LevelCurrentLevel { get; set; }

        public int LevelCurrentExperience { get; set; }

        public int LevelNeededExperience { get; set; }

        public double ExperiencePercent { get; set; }

        public long CharacteristicsAttack { get; set; }

        public long CharacteristicsDefense { get; set; }

        public long CharacteristicsMastery { get; set; }

        public long CharacteristicsMass { get; set; }

        public long StatisticsTotalFights { get; set; }

        public long StatisticsWins { get; set; }
    }
}
