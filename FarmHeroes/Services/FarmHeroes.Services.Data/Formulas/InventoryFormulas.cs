namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class InventoryFormulas
    {
        private const int UpgradeCostModifier = 500;

        public static Func<int, int> CalculateUpgradeCost = (level) => level * UpgradeCostModifier;
    }
}
