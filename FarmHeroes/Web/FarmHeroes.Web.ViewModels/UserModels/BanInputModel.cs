namespace FarmHeroes.Web.ViewModels.UserModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BanInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public DateTime BanUntil { get; set; }
    }
}
