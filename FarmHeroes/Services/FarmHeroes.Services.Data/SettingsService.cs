namespace FarmHeroes.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using FarmHeroes.Data.Common.Repositories;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Services.Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return this.settingsRepository.All().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.settingsRepository.All().To<T>().ToList();
        }
    }
}
