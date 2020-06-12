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

        public int CharacteristicsAttack { get; set; }

        public int CharacteristicsDefense { get; set; }

        public int CharacteristicsMastery { get; set; }

        public int CharacteristicsMass { get; set; }

        public int CharacteristicsDexterity { get; set; }

        public int StatisticsTotalFights { get; set; }

        public int StatisticsWins { get; set; }
    }
}
