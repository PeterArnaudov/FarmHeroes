namespace FarmHeroes.Services.Data.Tests.Common
{
    using System;

    using FarmHeroes.Data;
    using Microsoft.EntityFrameworkCore;

    public class FarmHeroesDbContextInMemoryInitializer
    {
        public static FarmHeroesDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<FarmHeroesDbContext>()
               .UseInMemoryDatabase(databaseName: "farm_heroes_db")
               .Options;

            return new FarmHeroesDbContext(options);
        }
    }
}
