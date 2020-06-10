namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="HealthService"/>.
    /// </summary>
    public static class HealthFormulas
    {
        /// <summary>
        /// Calculates the maximum health.
        /// </summary>
        /// <param name="mass">
        /// An <see cref="int"/>, the value of mass characteristic.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the maximum health.
        /// </returns>
        public static int CalculateMaximumHealth(int mass) =>
            mass * mass * 2;
    }
}
