namespace FarmHeroes.Services.Data.Formulas
{
    using System;

    public static class LevelFormulas
    {
        public static Func<int, int> CalculateNextExperienceNeeded = (previous) => (int)(previous * 1.15);
    }
}
