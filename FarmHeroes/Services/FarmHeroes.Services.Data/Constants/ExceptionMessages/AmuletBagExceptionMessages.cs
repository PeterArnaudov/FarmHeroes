namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;

    public class AmuletBagExceptionMessages
    {
        public const string AmuletBagAlreadyPurchasedMessage = "Cannot extend rent or purchase.";

        public const string AmuletBagAlreadyPurchasedInstruction = "You have purchased the amulet bag and it is active forever.";

        public const string HeroDoesNotOwnAmuletsMessage = "Cannot set amulet bag.";

        public const string HeroDoesNotOwnAmuletsInstruction = "You don't own the specified amulets.";
    }
}
