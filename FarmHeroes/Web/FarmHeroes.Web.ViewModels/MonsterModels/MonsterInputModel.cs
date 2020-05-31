namespace FarmHeroes.Web.ViewModels.MonsterModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MonsterInputModel
    {
        private const int MinimumLevel = 1;
        private const int MaximumLevel = 50;

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AvatarUrl { get; set; }

        [Required]
        [Range(MinimumLevel, MaximumLevel)]
        public int Level { get; set; }

        [Required]
        public int StatPercentage { get; set; }

        [Required]
        public int MinimalRewardModifier { get; set; }

        [Required]
        public int MaximalRewardModifier { get; set; }
    }
}
