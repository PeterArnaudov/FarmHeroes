namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;

    public static class MonsterFormulas
    {
        private const double AttackBattlePowerModifier = 2.6;
        private const double DefenseBattlePowerModifier = 2.35;
        private const double MasteryBattlePowerModifier = 2.5;
        private const int LevelOneMinimalModifier = 10;
        private const int LevelOneMaximalModifier = 100;
        private const int LevelTwoMinimalModifier = 10;
        private const int LevelTwoMaximalModifier = 110;
        private const int LevelThreeMinimalModifier = 15;
        private const int LevelThreeMaximalModifier = 110;
        private const int LevelFourMinimalModifier = 20;
        private const int LevelFourMaximalModifier = 130;
        private const int LevelFiveMinimalModifier = 50;
        private const int LevelFiveMaximalModifier = 250;
        private const int LevelSixMinimalModifier = 100;
        private const int LevelSixMaximalModifier = 550;
        private const int LevelSevenMinimalModifier = 250;
        private const int LevelSevenMaximalModifier = 750;
        private const int LevelEightMinimalModifier = 500;
        private const int LevelEightMaximalModifier = 5000;
        private const int LevelNineMinimalModifier = 500;
        private const int LevelNineMaximalModifier = 20000;

        public static Func<int, int, int> CalculateAttack = (battlePower, percent) => (int)(battlePower * (percent / 100d) / AttackBattlePowerModifier);

        public static Func<int, int, int> CalculateDefense = (battlePower, percent) => (int)(battlePower * (percent / 100d) / DefenseBattlePowerModifier);

        public static Func<int, int, int> CalculateMastery = (battlePower, percent) => (int)(battlePower * (percent / 100d) / MasteryBattlePowerModifier);

        public static Func<int, int, int> CalculateBattlePower = (heroBattlePower, percent) => (int)(heroBattlePower * (percent / 100d));

        public static Func<int, int, int> CalculateReward = (monsterLevel, heroLevel) =>
        {
            int minimalModifier = 0, maximalModifier = 0;

            if (monsterLevel == 1)
            {
                minimalModifier = LevelOneMinimalModifier;
                maximalModifier = LevelOneMaximalModifier;
            }
            else if (monsterLevel == 2)
            {
                minimalModifier = LevelTwoMinimalModifier;
                maximalModifier = LevelTwoMaximalModifier;
            }
            else if (monsterLevel == 3)
            {
                minimalModifier = LevelThreeMinimalModifier;
                maximalModifier = LevelThreeMaximalModifier;
            }
            else if (monsterLevel == 4)
            {
                minimalModifier = LevelFourMinimalModifier;
                maximalModifier = LevelFourMaximalModifier;
            }
            else if (monsterLevel == 5)
            {
                minimalModifier = LevelFiveMinimalModifier;
                maximalModifier = LevelFiveMaximalModifier;
            }
            else if (monsterLevel == 6)
            {
                minimalModifier = LevelSixMinimalModifier;
                maximalModifier = LevelSixMaximalModifier;
            }
            else if (monsterLevel == 7)
            {
                minimalModifier = LevelSevenMinimalModifier;
                maximalModifier = LevelSevenMaximalModifier;
            }
            else if (monsterLevel == 8)
            {
                minimalModifier = LevelEightMinimalModifier;
                maximalModifier = LevelEightMaximalModifier;
            }
            else if (monsterLevel == 9)
            {
                minimalModifier = LevelNineMinimalModifier;
                maximalModifier = LevelNineMaximalModifier;
            }

            Random random = new Random();
            int gold = random.Next(minimalModifier * heroLevel, maximalModifier * heroLevel);

            return gold;
        };
    }
}
