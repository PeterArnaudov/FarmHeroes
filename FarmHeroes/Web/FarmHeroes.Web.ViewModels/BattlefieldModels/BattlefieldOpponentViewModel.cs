namespace FarmHeroes.Web.ViewModels.BattlefieldModels
{
    using System;

    public class BattlefieldOpponentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public int CharacteristicsAttack { get; set; }

        public int CharacteristicsDefense { get; set; }

        public int CharacteristicsMass { get; set; }

        public int CharacteristicsMastery { get; set; }
    }
}
