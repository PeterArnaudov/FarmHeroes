namespace FarmHeroes.Web.Localization
{
    using Microsoft.Extensions.Localization;
    using System.Reflection;

    public class LocalizationService
    {
        private readonly IStringLocalizer amuletLocalizer;
        private readonly IStringLocalizer equipmentLocalizer;

        public LocalizationService(IStringLocalizerFactory factory)
        {
            var type = this.GetType();
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            this.amuletLocalizer = factory.Create("Amulets", assemblyName.Name);
            this.equipmentLocalizer = factory.Create("Equipment", assemblyName.Name);
        }

        public LocalizedString AmuletLocalizer(string key)
        {
            return this.amuletLocalizer[key];
        }

        public LocalizedString EquipmentLocalizer(string key)
        {
            return this.equipmentLocalizer[key];
        }
    }
}