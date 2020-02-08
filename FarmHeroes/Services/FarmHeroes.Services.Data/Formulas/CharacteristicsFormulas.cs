namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class CharacteristicsFormulas
    {
        private const int AttackModifier = 2;
        private const double DefenseModifier = 1.5;
        private const int MassModifier = 1;
        private const double MasteryModifier = 1.25;

        public static Func<int, int> CalculateAttackPrice = (amount) => amount * amount * AttackModifier;

        public static Func<int, int> CalculateDefensePrice = (amount) => (int)Math.Floor(amount * amount * DefenseModifier);

        public static Func<int, int> CalculateMassPrice = (amount) => amount * amount * MassModifier;

        public static Func<int, int> CalculateMasteryPrice = (amount) => (int)Math.Floor(amount * amount * MasteryModifier);
    }
}
