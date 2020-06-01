namespace FarmHeroes.Data.Seeding
{
    using FarmHeroes.Data.Models.DatabaseModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LevelsSeeder : ISeeder
    {
        private const double Modifier = 1.15;

        public async Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.DatabaseLevels.Any())
            {
                return;
            }

            List<DatabaseLevel> levels = new List<DatabaseLevel>();
            int neededExperience = 20;

            for (int i = 1; i <= 100; i++)
            {
                levels.Add(new DatabaseLevel()
                {
                    Level = i,
                    NeededExperience = neededExperience,
                });

                neededExperience = (int)(neededExperience * Modifier);
            }

            await dbContext.DatabaseLevels.AddRangeAsync(levels);
        }
    }
}
