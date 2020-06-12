namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using System;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="MonsterService"/>.
    /// </summary>
    public static class MonsterFormulas
    {
        private const double AttackBattlePowerModifier = 2.6;
        private const double DefenseBattlePowerModifier = 2.35;
        private const double MasteryBattlePowerModifier = 2.5;
        private const double DexterityBattlePowerModifier = 2.3;
        private const int InitialCharacteristicsModifier = 10;

        /// <summary>
        /// Calculates the value of the attack characteristic of the monster.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/>, the level of the monster.
        /// </param>
        /// <param name="battlePower">
        /// An <see cref="int"/>, the battle power of the monster.
        /// </param>
        /// <param name="percent">
        /// An <see cref="int"/>, the percent of battle power to be converted to attack.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the value of the monster's attack characteristic.
        /// </returns>
        public static int CalculateAttack(int level, int battlePower, int percent) =>
            (int)(battlePower * (percent / 100d) / AttackBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        /// <summary>
        /// Calculates the value of the defense characteristic of the monster.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/>, the level of the monster.
        /// </param>
        /// <param name="battlePower">
        /// An <see cref="int"/>, the battle power of the monster.
        /// </param>
        /// <param name="percent">
        /// An <see cref="int"/>, the percent of battle power to be converted to defense.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the value of the monster's defense characteristic.
        /// </returns>
        public static int CalculateDefense(int level, int battlePower, int percent) =>
            (int)(battlePower * (percent / 100d) / DefenseBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        /// <summary>
        /// Calculates the value of the mastery characteristic of the monster.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/>, the level of the monster.
        /// </param>
        /// <param name="battlePower">
        /// An <see cref="int"/>, the battle power of the monster.
        /// </param>
        /// <param name="percent">
        /// An <see cref="int"/>, the percent of battle power to be converted to mastery.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the value of the monster's mastery characteristic.
        /// </returns>
        public static int CalculateMastery(int level, int battlePower, int percent) =>
            (int)(battlePower * (percent / 100d) / MasteryBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        /// <summary>
        /// Calculates the value of the dexterity characteristic of the monster.
        /// </summary>
        /// <param name="level">
        /// An <see cref="int"/>, the level of the monster.
        /// </param>
        /// <param name="battlePower">
        /// An <see cref="int"/>, the battle power of the monster.
        /// </param>
        /// <param name="percent">
        /// An <see cref="int"/>, the percent of battle power to be converted to dexterity.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the value of the monster's dexterity characteristic.
        /// </returns>
        public static int CalculateDexterity(int level, int battlePower, int percent) =>
            (int)(battlePower * (percent / 100d) / DexterityBattlePowerModifier) + (level * InitialCharacteristicsModifier);

        /// <summary>
        /// Calculates the battle power of the monster.
        /// </summary>
        /// <param name="heroBattlePower">
        /// An <see cref="int"/>, the battle power of the hero.
        /// </param>
        /// <param name="percent">
        /// An <see cref="int"/>, the percentage of the hero's battle power the monster will have.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the monster's battle power.
        /// </returns>
        public static int CalculateBattlePower(int heroBattlePower, int percent) =>
            (int)(heroBattlePower * (percent / 100d));

        /// <summary>
        /// Calculates the gold received from the monster.
        /// </summary>
        /// <param name="monster">
        /// A <see cref="FarmHeroes.Data.Models.MonsterModels.Monster"/>, which the hero defeated.
        /// </param>
        /// <param name="heroLevel">
        /// An <see cref="int"/>, the level of the hero.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the gold reward from defeating the monster.
        /// </returns>
        public static int CalculateReward(Monster monster, int heroLevel)
        {
            Random random = new Random();
            int gold = random.Next(monster.MinimalRewardModifier * heroLevel, monster.MaximalRewardModifier * heroLevel);

            return gold;
        }
    }
}
