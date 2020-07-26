namespace FarmHeroes.Web.ViewModels.NewsModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class NewsInputModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [Url]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Content { get; set; }
    }
}
