namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="InventoryService"/>.
    /// </summary>
    public static class InventoryFormulas
    {
        private const int UpgradeCostModifier = 500;

        /// <summary>
        /// Calculates the upgrade cost of the inventory.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/> for <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/>'s <see cref="FarmHeroes.Data.Models.HeroModels.Level.CurrentLevel"/>.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the upgrade cost of the inventory.
        /// </returns>
        public static int CalculateUpgradeCost(int level) =>
            level * UpgradeCostModifier;
    }
}
