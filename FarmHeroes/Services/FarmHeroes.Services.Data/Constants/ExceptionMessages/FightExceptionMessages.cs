namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;

    public class FightExceptionMessages
    {
        public const string CannotAttackExceptionMessage = "You cannot attack.";

        public const string CannotAttackWhileWorkingInstruction = "You cannot attack while working. You have to cancel or finish your work before trying to attack.";

        public const string CannotAttackWhileRestingInstruction = "You have to wait until you can attack again. Take a rest, do something useful.";

        public const string CannotAttackThemselvesInstruction = "You cannot attack yourself. Go find somebody else.";

        public const string CannotAttackSameFractionInstruction = "You cannot attack a hero from your fraction, they are not enemies.";

        public const string CannotAttackOutsideOfLevelRangeInstruction = "The hero you attempted to attack is outside of your level range. You can attack players with 3 levels below or above you, inclusively.";

        public const string CannotAttackHeroesWithImmunity = "This hero still has defence. After being attacked, one is guaranteed an hour of immunity.";
    }
}
