namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="BattlefieldService"/>.
    /// </summary>
    public static class BattlefieldFormulas
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Calculates the gold gained from patrol.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/> for <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>'s <see cref="FarmHeroes.Data.Models.HeroModels.Level.CurrentLevel"/>.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the gold reward from patrol.
        /// </returns>
        public static int CalculatePatrolGold(int level) =>
            (int)(Math.Pow(level, 3) * 5 * (Random.NextDouble() + 1));

        /// <summary>
        /// Calculates the experience gained from patrol.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/> for <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>'s <see cref="FarmHeroes.Data.Models.HeroModels.Level.CurrentLevel"/>.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the experience reward from patrol.
        /// </returns>
        public static int CalculatePatrolExperience(int level) =>
            level * 2;
    }
}