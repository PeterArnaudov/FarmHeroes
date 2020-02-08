namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class HealthFormulas
    {
        public static Func<int, int> CalculateMaximumHealth = (mass) => mass * mass * 2;
    }
}
