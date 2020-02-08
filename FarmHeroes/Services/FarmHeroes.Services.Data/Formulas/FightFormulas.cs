namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class FightFormulas
    {
        private static Random Random = new Random();

        public static Func<int, bool, int> CalculateDamage = (attack, isCrit) =>
            isCrit ? (int)(attack * (Random.Next(75, 150) / 100d) * 1.5) : (int)(attack * (Random.Next(75, 150) / 100d));

        public static Func<int, int> CalculateBlocked = (defense) => (int)(defense * (Random.Next(50, 100) / 100d));

        public static Func<int, int, int, int, int> CalculateHitDamage = (attackerAttack, defenderDefense, attackerMastery, defenderMastery) =>
        {
            bool isCrit = IsCrit(attackerMastery, defenderMastery);
            int attackerDamage = CalculateDamage(attackerAttack, isCrit);
            int damageBlocked = CalculateBlocked(defenderDefense);
            int hitDamage = attackerDamage - damageBlocked;

            return hitDamage < 0 ? 0 : hitDamage;
        };

        public static Func<int, int, bool> IsCrit = (attackerMastery, defenderMastery) =>
        {
            double heroCritChance = Random.NextDouble() * attackerMastery / (attackerMastery + defenderMastery);
            double neededChance = Random.NextDouble();

            return heroCritChance >= neededChance ? true : false;
        };
}
}
