namespace FarmHeroes.Web.ViewModels.HeroModels
{
    using System.ComponentModel.DataAnnotations;

    using FarmHeroes.Data.Models.Enums;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class HeroCreateInputModel
    {
        [BindNever]
        public string Name { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public Fraction Fraction { get; set; }
    }
}
