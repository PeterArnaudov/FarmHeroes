namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="SmithService"/>.
    /// </summary>
    public static class SmithFormulas
    {
        /// <summary>
        /// Calculates the upgrade cost of an <see cref="FarmHeroes.Data.Models.HeroModels.HeroEquipment"/>.
        /// </summary>
        /// <param name="equipment">
        /// A <see cref="FarmHeroes.Data.Models.HeroModels.HeroEquipment"/>, upon which to be calculated the price.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the upgrade cost of the <see cref="FarmHeroes.Data.Models.HeroModels.HeroEquipment"/>.
        /// </returns>
        public static int CalculateEquipmentUpgradeCost(HeroEquipment equipment) =>
            equipment.RequiredLevel * (equipment.Level + 5);

        /// <summary>
        /// Calculates the upgrade cost of an <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/>.
        /// </summary>
        /// <param name="amulet">
        /// A <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/>, upon which to be calculated the price.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the upgrade cost of the <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/>.
        /// </returns>
        public static int CalculateAmuletUpgradeCost(HeroAmulet amulet) =>
            amulet.Level == 99 ? amulet.InitialPrice * 100 : amulet.InitialPrice;
    }
}
