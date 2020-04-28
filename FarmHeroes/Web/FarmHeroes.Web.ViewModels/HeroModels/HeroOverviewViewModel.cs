namespace FarmHeroes.Web.ViewModels.HeroModels
{
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;

    public class HeroOverviewViewModel
    {
        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public int LevelCurrentLevel { get; set; }

        public int LevelCurrentExperience { get; set; }

        public int LevelNeededExperience { get; set; }

        public double ExperiencePercent { get; set; }

        public EquipmentViewModel EquippedSetHelmet { get; set; }

        public EquipmentViewModel EquippedSetArmor{ get; set; }

        public EquipmentViewModel EquippedSetWeapon { get; set; }

        public EquipmentViewModel EquippedSetShield { get; set; }

        public AmuletViewModel EquippedSetAmulet { get; set; }

        public int CharacteristicsAttack { get; set; }

        public int CharacteristicsDefense { get; set; }

        public int CharacteristicsMastery { get; set; }

        public int CharacteristicsMass { get; set; }

        public int StatisticsTotalFights { get; set; }

        public int StatisticsWins { get; set; }
    }
}
