namespace FarmHeroes.Data.Models.MonsterModels
{
    using System;

    public class Monster
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public int Level { get; set; }

        public int StatPercentage { get; set; }
    }
}
