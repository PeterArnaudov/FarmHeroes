namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;

    public class SmithExceptionMessages
    {
        public const string ItemNotOwnedMessage = "You cannot upgrade an item that isn't in your inventory.";

        public const string ItemNotOwnedInstruction = "Choose an item from your inventory.";

        public const string ItemFullyUpgradedMessage = "This item is already upgraded to its maximum level.";

        public const string ItemFullyUpgradedInstruction = "Choose an item that isn't fully upgraded.";
    }
}
