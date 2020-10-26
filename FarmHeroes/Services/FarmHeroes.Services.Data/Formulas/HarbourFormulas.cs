namespace FarmHeroes.Services.Data.Formulas
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class HarbourFormulas
    {
        public static int CalculateFishCatch(int minimumCatch, int maximumCatch)
        {
            Random random = new Random();

            return random.Next(minimumCatch, maximumCatch);
        }
    }
}
