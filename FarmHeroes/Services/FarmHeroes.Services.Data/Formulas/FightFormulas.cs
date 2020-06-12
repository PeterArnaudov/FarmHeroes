namespace FarmHeroes.Services.Data.Formulas
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Linq;

    /// <summary>
    /// Contains methods for performing calculations related to the <see cref="FightService"/>.
    /// </summary>
    public static class FightFormulas
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Calculates the damage inflicted.
        /// </summary>
        /// <param name="attack">
        /// An <see cref="int"/>, the value of attack characteristic.
        /// </param>
        /// <param name="isCrit">
        /// A <see cref="bool"/> that specifies whether the hit is critical or not.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the damage inflicted.
        /// </returns>
        public static int CalculateDamage(int attack, bool isCrit) =>
            isCrit ? (int)(attack * (Random.Next(75, 150) / 100d) * 1.5) : (int)(attack * (Random.Next(75, 150) / 100d));

        /// <summary>
        /// Calculates the damage blocked.
        /// </summary>
        /// <param name="defense">
        /// An <see cref="int"/>, the value of defense characteristic.
        /// </param>
        /// <param name="blockAmuletBonus">
        /// A <see cref="double"/>, the bonus from <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/>.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the damage blocked.
        /// </returns>
        public static int CalculateBlocked(int defense, double blockAmuletBonus)
        {
            int blocked = (int)(defense * (Random.Next(50, 100) / 100d));
            blocked = (int)(blocked * (1 + (blockAmuletBonus / 100)));

            return blocked;
        }

        /// <summary>
        /// A method that calls other <see cref="FightFormulas"/> methods to calculate the final hit damage.
        /// </summary>
        /// <param name="attackerAttack">
        /// An <see cref="int"/>, the value of attack characteristic of the attacker.
        /// </param>
        /// <param name="defenderDefense">
        /// An <see cref="int"/>, the value of defender characteristic of the defender.
        /// </param>
        /// <param name="isCrit">
        /// A <see cref="bool"/>, specifies whether the hit is critical or not.
        /// </param>
        /// <param name="isDodged">
        /// A <see cref="bool"/>, specifies whether the hit is dodged or not.
        /// </param>
        /// <param name="blockAmuletBonus">
        /// A <see cref="double"/>, the bonus from <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/> of the defender.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the final damage of a hit.
        /// </returns>
        public static int CalculateHitDamage(int attackerAttack, int defenderDefense, bool isCrit, bool isDodged, double blockAmuletBonus)
        {
            int attackerDamage = CalculateDamage(attackerAttack, isCrit);
            int damageBlocked = CalculateBlocked(defenderDefense, blockAmuletBonus);
            int hitDamage = attackerDamage - damageBlocked;

            return hitDamage < 0 || isDodged ? 0 : hitDamage;
        }

        /// <summary>
        /// Calculates whether the hit is critical or not.
        /// </summary>
        /// <param name="attackerMastery">
        /// An <see cref="int"/>, the value of mastery characteristic of the attacker.
        /// </param>
        /// <param name="defenderDexterity">
        /// An <see cref="int"/>, the value of dexterity characteristic of the defender.
        /// </param>
        /// <param name="amuletBonus">
        /// A <see cref="double"/>, the bonus from <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/> of the attacker.
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> whether the hit is critical or not.
        /// </returns>
        public static bool IsCrit(int attackerMastery, int defenderDexterity, double amuletBonus)
        {
            double heroCritChance = Random.Next(0, 100) * attackerMastery / (attackerMastery + defenderDexterity);
            heroCritChance *= 1 + (amuletBonus / 100);
            double neededChance = Random.Next(0, 100);

            return heroCritChance >= neededChance ? true : false;
        }

        /// <summary>
        /// Calculates whether the hit is dodged or not.
        /// </summary>
        /// <param name="attackerAttack">
        /// An <see cref="int"/>, the value of attack characteristic of the attacker.
        /// </param>
        /// <param name="defenderDexterity">
        /// An <see cref="int"/>, the value of dexterity characteristic of the defender.
        /// </param>
        /// <param name="amuletBonus">
        /// A <see cref="double"/>, the bonus from <see cref="FarmHeroes.Data.Models.HeroModels.HeroAmulet"/> of the defender.
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> whether the hit is dodged or not.
        /// </returns>
        public static bool IsDodged(int attackerAttack, int defenderDexterity, double amuletBonus)
        {
            double heroHitChance = ((double)attackerAttack / (attackerAttack + defenderDexterity)) * 100;
            heroHitChance /= 1 + (amuletBonus / 100);
            double neededChance = Random.Next(0, 100);

            return heroHitChance >= neededChance ? false : true;
        }

        /// <summary>
        /// Calculates the attack characteristic with all multipliers.
        /// </summary>
        /// <param name="hero">
        /// A <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> for which the attack characteristic will be calculated.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the attack characteristic of <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> in a fight.
        /// </returns>
        public static int CalculateAttack(Hero hero)
        {
            int attackFromCharacteristics = hero.Characteristics.Attack;

            int? weaponLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Weapon)?.Level;
            double percentFromWeaponLevel = weaponLevel == null ? 1 : 1 + ((int)weaponLevel / 100d);
            int attackFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Attack) * percentFromWeaponLevel);

            int attack = attackFromCharacteristics + attackFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(attack * percentFromBonuses);
        }

        /// <summary>
        /// Calculates the defense characteristic with all multipliers.
        /// </summary>
        /// <param name="hero">
        /// A <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> for which the defense characteristic will be calculated.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the defense characteristic of <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> in a fight.
        /// </returns>
        public static int CalculateDefense(Hero hero)
        {
            int defenseFromCharacteristics = hero.Characteristics.Defense;

            int? shieldLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Shield)?.Level;
            double percentFromShieldLevel = shieldLevel == null ? 1 : 1 + ((int)shieldLevel / 100d);
            int defenseFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Defense) * percentFromShieldLevel);

            int defense = defenseFromCharacteristics + defenseFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(defense * percentFromBonuses);
        }

        /// <summary>
        /// Calculates the mastery characteristic with all multipliers.
        /// </summary>
        /// <param name="hero">
        /// A <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> for which the mastery characteristic will be calculated.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the mastery characteristic of <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> in a fight.
        /// </returns>
        public static int CalculateMastery(Hero hero)
        {
            int masteryFromCharacteristics = hero.Characteristics.Defense;

            int? helmetLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Helmet)?.Level;
            double percentFromHelmetLevel = helmetLevel == null ? 1 : 1 + ((int)helmetLevel / 100d);
            int masteryFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Mastery) * percentFromHelmetLevel);

            int mastery = masteryFromCharacteristics + masteryFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(mastery * percentFromBonuses);
        }

        /// <summary>
        /// Calculates the dexterity characteristic with all multipliers.
        /// </summary>
        /// <param name="hero">
        /// A <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> for which the dexterity characteristic will be calculated.
        /// </param>
        /// <returns>
        /// An <see cref="int"/>, the dexterity characteristic of <see cref="FarmHeroes.Data.Models.HeroModels.Hero"/> in a fight.
        /// </returns>
        public static int CalculateDexterity(Hero hero)
        {
            int dexterityFromCharacteristics = hero.Characteristics.Dexterity;

            int? armorLevel = hero.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Armor)?.Level;
            double percentFromArmorLevel = armorLevel == null ? 1 : 1 + ((int)armorLevel / 100d);
            int dexterityFromSet = (int)(hero.EquippedSet.Equipped.Sum(x => x.Dexterity) * percentFromArmorLevel);

            int dexterity = dexterityFromCharacteristics + dexterityFromSet;

            double percentFromBonuses = 1 + hero.Inventory.Bonuses.Where(b => b.Type == BonusType.Characteristics && b.ActiveUntil > DateTime.UtcNow).Sum(b => b.Bonus);

            return (int)(dexterity * percentFromBonuses);
        }
    }
}
