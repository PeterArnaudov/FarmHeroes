namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="FarmService"/>.
    /// </summary>
    public static class FarmFormulas
    {
        /// <summary>
        /// Calculates the gold gained for one hour work on the farm.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/> for <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>'s <see cref="FarmHeroes.Data.Models.HeroModels.Level.CurrentLevel"/>.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the gold rewarded for an hour work on the farm.
        /// </returns>
        public static int CalculateFarmSalaryPerHour(int level) =>
            level * level * level;

        /// <summary>
        /// Calculates the gold gained for finished work on the farm.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/> for <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>'s <see cref="FarmHeroes.Data.Models.HeroModels.Level.CurrentLevel"/>.
        /// </param>
        /// <param name="hours">
        /// An <see cref="int"/>, the hours worked on the farm.
        /// </param>
        /// <param name="amuletBonus">
        /// A <see cref="double"/>, the bonus from <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/>.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the gold rewarded for finished work on the farm.
        /// </returns>
        public static int CalculateGoldEarned(int level, int hours, double amuletBonus) =>
            (int)(CalculateFarmSalaryPerHour(level) * hours * (1 + (amuletBonus / 100)));

        /// <summary>
        /// Calculates the experience gained from the farm.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/> for <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>'s <see cref="FarmHeroes.Data.Models.HeroModels.Level.CurrentLevel"/>.
        /// </param>
        /// <param name="hours">
        /// An <see cref="int"/>, the hours worked on the farm.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the experience gained for finished work on the farm.
        /// </returns>
        public static int CalculateExperience(int level, int hours) =>
            level * hours;
    }
}
