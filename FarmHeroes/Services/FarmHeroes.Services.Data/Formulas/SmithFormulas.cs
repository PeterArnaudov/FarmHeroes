namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;

    public static class SmithFormulas
    {
        public static Func<HeroEquipment, int> CalculateEquipmentUpgradeCost = (equipment) => equipment.RequiredLevel * (equipment.Level + 5);
    }
}
