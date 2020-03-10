namespace FarmHeroes.Web.ViewModels.HeroModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FarmHeroes.Data.Models.Enums;

    public class HeroModifyBasicInfoInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Fraction Fraction { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}
