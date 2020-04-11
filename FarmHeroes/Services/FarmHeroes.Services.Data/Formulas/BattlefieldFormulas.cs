namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class BattlefieldFormulas
    {
        private static Random Random = new Random();

        public static Func<int, int> CalculatePatrolGold = (level) => (int)(Math.Pow(level, 3) * 5 * (Random.NextDouble() + 1));

        public static Func<int, int> CalculatePatrolExperience = (level) => level * 2;
    }
}
