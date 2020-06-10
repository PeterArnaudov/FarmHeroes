namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="ResourcePouchService"/>.
    /// </summary>
    public static class ResourceFormulas
    {
        /// <summary>
        /// Calculates the gold to be stolen from a hero.
        /// </summary>
        /// <param name="amount">
        /// An <see cref="int"/>, the amount of gold the hero has.
        /// </param>
        /// <param name="safe">
        /// A <see cref="double"/>, the percentage gold safe protects.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the amount of stolen gold.
        /// </returns>
        public static int CalculateStolenGold(int amount, double safe) =>
            (int)(amount * 0.1 * (1 - safe));

        /// <summary>
        /// Calculates the amount of gold to be received from the passive income.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/>, the level of the hero.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the amount of gold to be received.
        /// </returns>
        public static int CalculatePassiveIncome(int level) =>
            level * level * 5;
    }
}
