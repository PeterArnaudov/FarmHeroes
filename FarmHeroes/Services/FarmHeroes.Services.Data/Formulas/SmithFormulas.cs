namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    using FarmHeroes.Data.Models.HeroModels;

    public static class SmithFormulas
    {
        public static Func<HeroEquipment, int> CalculateEquipmentUpgradeCost = (equipment) => equipment.RequiredLevel * (equipment.Level + 5);

        public static Func<HeroAmulet, int> CalculateAmuletUpgradeCost = (amulet) => amulet.Level == 99 ? amulet.InitialPrice * 100 : amulet.InitialPrice;
    }
}
