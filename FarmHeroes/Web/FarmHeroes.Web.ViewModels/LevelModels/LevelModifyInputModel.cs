namespace FarmHeroes.Web.ViewModels.LevelModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LevelModifyInputModel
    {
        public string Name { get; set; }

        [Required]
        public int LevelCurrentLevel { get; set; }

        [Required]
        public int LevelCurrentExperience { get; set; }

        [Required]
        public int LevelNeededExperience { get; set; }
    }
}
