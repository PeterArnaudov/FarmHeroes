namespace FarmHeroes.Web.ViewModels.HeroModels
{
    using FarmHeroes.Data.Models.Enums;

    public class HeroCreateInputModel
    {
        public string Name { get; set; }

        public Gender Gender { get; set; }

        public Fraction Fraction { get; set; }
    }
}
