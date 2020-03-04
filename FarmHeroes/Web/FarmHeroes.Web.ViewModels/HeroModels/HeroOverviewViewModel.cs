namespace FarmHeroes.Web.ViewModels.HeroModels
{
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;

    public class HeroOverviewViewModel
    {
        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public int Level { get; set; }

        public int CurrentExperience { get; set; }

        public int NeededExperience { get; set; }

        public double ExperiencePercent { get; set; }

        public EquipmentViewModel EquippedSetHelmet { get; set; }

        public EquipmentViewModel EquippedSetArmor{ get; set; }

        public EquipmentViewModel EquippedSetWeapon { get; set; }

        public EquipmentViewModel EquippedSetShield { get; set; }

        public AmuletViewModel EquippedSetAmulet { get; set; }

        public long Attack { get; set; }

        public long Defense { get; set; }

        public long Mastery { get; set; }

        public long Mass { get; set; }

        public long Fights { get; set; }

        public long Wins { get; set; }
    }
}
