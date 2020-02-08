namespace FarmHeroes.Data.Models.HeroModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EquippedSet
    {
        public int Id { get; set; }

        public int? HelmetId { get; set; }

        public virtual HeroHelmet Helmet { get; set; }

        public int? ArmorId { get; set; }

        public virtual HeroArmor Armor { get; set; }

        public int? WeaponId { get; set; }

        public virtual HeroWeapon Weapon { get; set; }

        public int? ShieldId { get; set; }

        public virtual HeroShield Shield { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
