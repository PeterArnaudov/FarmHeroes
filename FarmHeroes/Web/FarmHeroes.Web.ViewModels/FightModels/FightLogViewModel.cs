namespace FarmHeroes.Web.ViewModels.FightModels
{
    using FarmHeroes.Data.Models.FightModels;
    using System;

    public class FightLogViewModel
    {
        public int AttackerId { get; set; }

        public int DefenderId { get; set; }

        public string AttackerName { get; set; }

        public string DefenderName { get; set; }

        public int AttackerAttack { get; set; }

        public int AttackerDefense { get; set; }

        public int AttackerMastery { get; set; }

        public int AttackerMass { get; set; }

        public int DefenderAttack { get; set; }

        public int DefenderDefense { get; set; }

        public int DefenderMastery { get; set; }

        public int DefenderMass { get; set; }

        public int AttackerDamageDealt { get; set; }

        public int DefenderDamageDealt { get; set; }

        public int AttackerHealthLeft { get; set; }

        public int DefenderHealthLeft { get; set; }

        public string WinnerName { get; set; }

        public int GoldStolen { get; set; }

        public int ExperienceGained { get; set; }

        public int?[] AttackerHits { get; set; } = new int?[5];

        public int?[] DefenderHits { get; set; } = new int?[5];
    }
}
