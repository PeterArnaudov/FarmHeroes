namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class FightFormulas
    {
        private static Random Random = new Random();

        public static Func<int, bool, int> CalculateDamage = (attack, isCrit) =>
            isCrit ? (int)(attack * (Random.Next(75, 150) / 100d) * 1.5) : (int)(attack * (Random.Next(75, 150) / 100d));

        public static Func<int, double, int> CalculateBlocked = (defense, blockAmuletBonus) =>
        {
            int blocked = (int)(defense * (Random.Next(50, 100) / 100d));
            blocked = (int)(blocked * (1 + (blockAmuletBonus / 100)));

            return blocked;
        };

        public static Func<int, int, int, int, double, double, int> CalculateHitDamage = (attackerAttack, defenderDefense, attackerMastery, defenderMastery, critAmuletBonus, blockAmuletBonus) =>
        {
            bool isCrit = IsCrit(attackerMastery, defenderMastery, critAmuletBonus);
            int attackerDamage = CalculateDamage(attackerAttack, isCrit);
            int damageBlocked = CalculateBlocked(defenderDefense, blockAmuletBonus);
            int hitDamage = attackerDamage - damageBlocked;

            return hitDamage < 0 ? 0 : hitDamage;
        };

        public static Func<int, int, double, bool> IsCrit = (attackerMastery, defenderMastery, amuletBonus) =>
        {
            double heroCritChance = Random.Next(0, 100) * attackerMastery / (attackerMastery + defenderMastery);
            heroCritChance *= 1 + (amuletBonus / 100);
            double neededChance = Random.Next(0, 100);

            return heroCritChance >= neededChance ? true : false;
        };

        public static Func<Hero, int> CalculateAttack = (hero) =>
        {
            int attackFromCharacteristics = hero.Characteristics.Attack;

            int? weaponLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Weapon)?.Level;
            double percentFromWeaponLevel = weaponLevel == null ? 1 : 1 + ((int)weaponLevel / 100d);
            int attackFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Attack) * percentFromWeaponLevel);

            int attack = attackFromCharacteristics + attackFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(attack * percentFromBonuses);
        };

        public static Func<Hero, int> CalculateDefense = (hero) =>
        {
            int defenseFromCharacteristics = hero.Characteristics.Defense;

            int? shieldLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Shield)?.Level;
            double percentFromShieldLevel = shieldLevel == null ? 1 : 1 + ((int)shieldLevel / 100d);
            int defenseFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Defense) * percentFromShieldLevel);

            int defense = defenseFromCharacteristics + defenseFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(defense * percentFromBonuses);
        };

        public static Func<Hero, int> CalculateMastery = (hero) =>
        {
            int masteryFromCharacteristics = hero.Characteristics.Defense;

            int? helmetLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Helmet)?.Level;
            double percentFromHelmetLevel = helmetLevel == null ? 1 : 1 + ((int)helmetLevel / 100d);
            int masteryFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Defense) * percentFromHelmetLevel);

            int mastery = masteryFromCharacteristics + masteryFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(mastery * percentFromBonuses);
        };
    }
}
