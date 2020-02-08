namespace FarmHeroes.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(FarmHeroesDbContext dbContext, IServiceProvider serviceProvider);
    }
}
