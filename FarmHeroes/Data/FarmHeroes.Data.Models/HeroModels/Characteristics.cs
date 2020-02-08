namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Characteristics
    {
        private const int InitialCharacteristicsAmount = 5;

        public int Id { get; set; }

        public int Attack { get; set; } = InitialCharacteristicsAmount;

        public int Defense { get; set; } = InitialCharacteristicsAmount;

        public int Mass { get; set; } = InitialCharacteristicsAmount;

        public int Mastery { get; set; } = InitialCharacteristicsAmount;

        public virtual Hero Hero { get; set; }
    }
}
