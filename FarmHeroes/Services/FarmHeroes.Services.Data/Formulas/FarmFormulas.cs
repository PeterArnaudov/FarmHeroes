namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class FarmFormulas
    {
        public static Func<int, int> CalculateFarmSalaryPerHour = (level) => level * level * level;

        public static Func<int, int, double, int> CalculateGoldEarned = (level, hours, amuletBonus) => (int)(CalculateFarmSalaryPerHour(level) * hours * (1 + (amuletBonus / 100)));

        public static Func<int, int, int> CalculateExperience = (level, hours) => level * hours;
    }
}
