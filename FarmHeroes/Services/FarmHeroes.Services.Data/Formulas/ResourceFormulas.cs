namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class ResourceFormulas
    {
        public static Func<int, double, int> CalculateStolenGold = (amount, safe) => (int)(amount * 0.1 * (1 - safe));

        public static Func<int, int> CalculatePassiveIncome = level => level * level * 5;
    }
}
