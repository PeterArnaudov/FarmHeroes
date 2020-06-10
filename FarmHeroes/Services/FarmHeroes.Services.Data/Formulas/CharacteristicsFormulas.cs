namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="CharacteristicsService"/>.
    /// </summary>
    public static class CharacteristicsFormulas
    {
        private const int AttackPriceModifier = 2;
        private const double DefensePriceModifier = 1.5;
        private const int MassPriceModifier = 1;
        private const double MasteryPriceModifier = 1.25;
        private const double AttackBattlePowerModifier = 2.6;
        private const double DefenseBattlePowerModifier = 2.35;
        private const double MasteryBattlePowerModifier = 2.5;

        /// <summary>
        /// Calculates the price of the attack characteristic.
        /// </summary>
        /// <param name="amount">
        /// An <see cref="int"/>, the amount with which the attack characteristics will be increased.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the price of attack characteristic.
        /// </returns>
        public static int CalculateAttackPrice(int amount) =>
            amount * amount * AttackPriceModifier;

        /// <summary>
        /// Calculates the price of the defense characteristic.
        /// </summary>
        /// <param name="amount">
        /// An <see cref="int"/>, the amount with which the defense characteristics will be increased.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the price of defense characteristic.
        /// </returns>
        public static int CalculateDefensePrice(int amount) =>
            (int)Math.Floor(amount * amount * DefensePriceModifier);

        /// <summary>
        /// Calculates the price of the mass characteristic.
        /// </summary>
        /// <param name="amount">
        /// An <see cref="int"/>, the amount with which the mass characteristics will be increased.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the price of mass characteristic.
        /// </returns>
        public static int CalculateMassPrice(int amount) =>
            amount * amount * MassPriceModifier;

        /// <summary>
        /// Calculates the price of the mastery characteristic.
        /// </summary>
        /// <param name="amount">
        /// An <see cref="int"/>, the amount with which the mastery characteristics will be increased.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the price of mastery characteristic.
        /// </returns>
        public static int CalculateMasteryPrice(int amount) =>
            (int)Math.Floor(amount * amount * MasteryPriceModifier);

        /// <summary>
        /// Calculates the battle power of a <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>.
        /// </summary>
        /// <param name="characteristics">
        /// The <see cref="FarmHeroes.Data.Models.HeroModels.Characteristics"/> of a <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>.
        /// </param>
        /// <returns>
        /// The total battle power of a <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>.
        /// </returns>
        public static int CalculateBattlePower(Characteristics characteristics) =>
            (int)(characteristics.Attack * AttackBattlePowerModifier) +
            (int)(characteristics.Defense * DefenseBattlePowerModifier) +
            (int)(characteristics.Mastery * MasteryBattlePowerModifier);
    }
}
