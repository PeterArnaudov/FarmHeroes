namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class FarmFormulas
    {
        private const int FarmExperience = 2;

        public static Func<int, int> CalculateFarmSalaryPerHour = (level) => level * level * level;

        public static Func<int, int, int> CalculateGoldEarned = (level, hours) => CalculateFarmSalaryPerHour(level) * hours;

        public static Func<int, int, int> CalculateExperience = (level, hours) => level * FarmExperience * hours;
    }
}
