namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class ResourceFormulas
    {
        public static Func<int, int> CalculateStolenGold = (amount) => (int)(amount * 0.1);
    }
}
