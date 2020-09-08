namespace FarmHeroes.Services.Data
{
    using Microsoft.Extensions.Localization;
    using System.Reflection;

    public class LocalizationService
    {
        private readonly IStringLocalizer amuletLocalizer;
        private readonly IStringLocalizer equipmentLocalizer;
        private readonly IStringLocalizer exceptionLocalizer;

        public LocalizationService(IStringLocalizerFactory factory)
        {
            var type = this.GetType();
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            this.amuletLocalizer = factory.Create("Amulets", assemblyName.Name);
            this.equipmentLocalizer = factory.Create("Equipment", assemblyName.Name);
            this.exceptionLocalizer = factory.Create("Exceptions", assemblyName.Name);
        }

        public LocalizedString AmuletLocalizer(string key)
        {
            return this.amuletLocalizer[key];
        }

        public LocalizedString EquipmentLocalizer(string key)
        {
            return this.equipmentLocalizer[key];
        }

        public LocalizedString ExceptionLocalizer(string key)
        {
            return this.exceptionLocalizer[key];
        }
    }
}