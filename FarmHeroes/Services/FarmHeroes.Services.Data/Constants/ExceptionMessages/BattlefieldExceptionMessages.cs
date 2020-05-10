namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;

    public class BattlefieldExceptionMessages
    {
        public const string CannotPatrolMessage = "You cannot go on a patrol.";

        public const string PatrolLimitInstruction = "You've already been on patrol the maximum allowed times today.";

        public const string CannotCollectRewardMessage = "You cannot collect your reward yet.";

        public const string CannotCollectRewardInstruction = "You have to finish your patrol before trying to collect your reward.";

        public const string CannotAttackMessage = "You cannot attack right now.";

        public const string CannotAttackWhileWorkingInstruction = "You have to cancel or finish your work before trying to attack.";

        public const string CannotAttackWhileRestingInstruction = "You have to wait until you can initiate a fight again.";

        public const string NoEnemiesAvailableMessage = "There are no heroes that you can attack right now.";

        public const string NoEnemiesAvailableInstruction = "There might be no heroes in your level range or all are with attack immunity.";
    }
}
