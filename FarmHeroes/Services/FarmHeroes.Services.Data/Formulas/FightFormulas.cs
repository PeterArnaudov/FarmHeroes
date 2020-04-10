namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using System;
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

        public static Func<EquippedSet, int> CalculateAttackFromSet = (equippedSet) =>
        {
            int? level = equippedSet.Equipped.Find(x => x.Type == EquipmentType.Weapon)?.Level;
            double? percent = 1 + (level / 100d);
            double actualPercent = percent ?? 1;
            int equipmentBonus = equippedSet.Equipped.Sum(x => x.Attack);
            return (int)(equipmentBonus * actualPercent);
        };

        public static Func<EquippedSet, int> CalculateDefenseFromSet = (equippedSet) =>
        {
            int? level = equippedSet.Equipped.Find(x => x.Type == EquipmentType.Shield)?.Level;
            double? percent = 1 + (level / 100d);
            double actualPercent = percent ?? 1;
            int equipmentBonus = equippedSet.Equipped.Sum(x => x.Defense);
            return (int)(equipmentBonus * actualPercent);
        };

        public static Func<EquippedSet, int> CalculateMasteryFromSet = (equippedSet) =>
        {
            int? level = equippedSet.Equipped.Find(x => x.Type == EquipmentType.Helmet)?.Level;
            double? percent = 1 + (level / 100d);
            double actualPercent = percent ?? 1;
            int equipmentBonus = equippedSet.Equipped.Sum(x => x.Mastery);
            return (int)(equipmentBonus * actualPercent);
        };
    }
}
