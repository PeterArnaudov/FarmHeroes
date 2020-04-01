namespace FarmHeroes.Services.Data.Tests.Common
{
    using System.Reflection;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Mapping;
    using FarmHeroes.Services.Models.Monsters;
    using FarmHeroes.Web.ViewModels.HeroModels;

    public class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(HeroCreateInputModel).GetTypeInfo().Assembly,
                typeof(Hero).GetTypeInfo().Assembly);
        }
    }
}
