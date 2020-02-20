namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;

    public static class CharacteristicsFormulas
    {
        private const int AttackPriceModifier = 2;
        private const double DefensePriceModifier = 1.5;
        private const int MassPriceModifier = 1;
        private const double MasteryPriceModifier = 1.25;
        private const double AttackBattlePowerModifier = 2.6;
        private const double DefenseBattlePowerModifier = 2.35;
        private const double MasteryBattlePowerModifier = 2.5;

        public static Func<int, int> CalculateAttackPrice = (amount) => amount * amount * AttackPriceModifier;

        public static Func<int, int> CalculateDefensePrice = (amount) => (int)Math.Floor(amount * amount * DefensePriceModifier);

        public static Func<int, int> CalculateMassPrice = (amount) => amount * amount * MassPriceModifier;

        public static Func<int, int> CalculateMasteryPrice = (amount) => (int)Math.Floor(amount * amount * MasteryPriceModifier);

        public static Func<Characteristics, int> CalculateBattlePower = (characteristics) =>
            (int)(characteristics.Attack * AttackBattlePowerModifier) +
            (int)(characteristics.Defense* DefenseBattlePowerModifier) +
            (int)(characteristics.Mastery * MasteryBattlePowerModifier);
    }
}
