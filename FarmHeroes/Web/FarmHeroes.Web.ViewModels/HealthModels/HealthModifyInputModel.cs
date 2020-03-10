namespace FarmHeroes.Web.ViewModels.HealthModels
{
    using System.ComponentModel.DataAnnotations;

    public class HealthModifyInputModel
    {
        public string Name { get; set; }

        [Required]
        public int HealthCurrent { get; set; }

        [Required]
        public int HealthMaximum { get; set; }
    }
}
