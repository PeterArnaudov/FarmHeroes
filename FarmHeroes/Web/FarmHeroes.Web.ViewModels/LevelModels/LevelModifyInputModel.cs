namespace FarmHeroes.Web.ViewModels.LevelModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LevelModifyInputModel
    {
        public string Name { get; set; }

        [Required]
        [Range(1, 100)]
        public int LevelCurrentLevel { get; set; }
    }
}
