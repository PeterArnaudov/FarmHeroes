﻿namespace FarmHeroes.Web.ViewModels.FightModels
{
    using FarmHeroes.Data.Models.FightModels;

    public class FightLogViewModel
    {
        public int AttackerId { get; set; }

        public int DefenderId { get; set; }

        public string AttackerName { get; set; }

        public string DefenderName { get; set; }

        public int AttackerLevel { get; set; }

        public int DefenderLevel { get; set; }

        public string AttackerAvatarUrl { get; set; }

        public string DefenderAvatarUrl { get; set; }

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
    }
}
