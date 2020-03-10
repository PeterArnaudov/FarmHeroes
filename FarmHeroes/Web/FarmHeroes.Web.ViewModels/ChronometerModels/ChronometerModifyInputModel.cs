namespace FarmHeroes.Web.ViewModels.ChronometerModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChronometerModifyInputModel
    {
        public string Name { get; set; }

        public DateTime? ChronometerWorkUntil { get; set; }

        [Required]
        public DateTime ChronometerCannotAttackHeroUntil { get; set; }

        [Required]
        public DateTime ChronometerCannotAttackMonsterUntil { get; set; }

        [Required]
        public DateTime ChronometerCannotBeAttackedUntil { get; set; }
    }
}
