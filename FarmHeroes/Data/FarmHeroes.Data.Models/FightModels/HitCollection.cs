namespace FarmHeroes.Data.Models.FightModels
{
    using System;

    public class HitCollection
    {
        public int Id { get; set; }

        public int? AttackerHitOne { get; set; }

        public int? AttackerHitTwo { get; set; }

        public int? AttackerHitThree { get; set; }

        public int? AttackerHitFour { get; set; }

        public int? AttackerHitFive { get; set; }

        public int? DefenderHitOne { get; set; }

        public int? DefenderHitTwo { get; set; }

        public int? DefenderHitThree { get; set; }

        public int? DefenderHitFour { get; set; }

        public int? DefenderHitFive { get; set; }

        public virtual Fight Fight { get; set; }

        public int?[][] ToArray()
        {
            int?[][] array = new int?[2][];
            array[0] = new int?[5];
            array[1] = new int?[5];

            array[0][0] = this.AttackerHitOne;
            array[0][1] = this.AttackerHitTwo;
            array[0][2] = this.AttackerHitThree;
            array[0][3] = this.AttackerHitFour;
            array[0][4] = this.AttackerHitFive;
            array[1][0] = this.DefenderHitOne;
            array[1][1] = this.DefenderHitTwo;
            array[1][2] = this.DefenderHitThree;
            array[1][3] = this.DefenderHitFour;
            array[1][4] = this.DefenderHitFive;

            return array;
        }
    }
}
