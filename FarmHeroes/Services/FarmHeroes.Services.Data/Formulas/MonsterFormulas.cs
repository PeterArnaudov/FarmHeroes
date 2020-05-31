namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using System;

    public static class MonsterFormulas
    {
        private const double AttackBattlePowerModifier = 2.6;
        private const double DefenseBattlePowerModifier = 2.35;
        private const double MasteryBattlePowerModifier = 2.5;
        private const int InitialCharacteristicsModifier = 10;

        public static Func<int, int, int, int> CalculateAttack = (level, battlePower, percent) => (int)(battlePower * (percent / 100d) / AttackBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        public static Func<int, int, int, int> CalculateDefense = (level, battlePower, percent) => (int)(battlePower * (percent / 100d) / DefenseBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        public static Func<int, int, int, int> CalculateMastery = (level, battlePower, percent) => (int)(battlePower * (percent / 100d) / MasteryBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        public static Func<int, int, int> CalculateBattlePower = (heroBattlePower, percent) => (int)(heroBattlePower * (percent / 100d));

        public static Func<Monster, int, int> CalculateReward = (monster, heroLevel) =>
        {
            Random random = new Random();
            int gold = random.Next(monster.MinimalRewardModifier * heroLevel, monster.MaximalRewardModifier * heroLevel);

            return gold;
        };
    }
}
