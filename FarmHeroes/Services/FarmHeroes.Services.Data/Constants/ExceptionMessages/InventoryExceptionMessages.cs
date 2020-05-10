namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;

    public class InventoryExceptionMessages
    {
        public const string ItemNotOwnedMessage = "You cannot trash an item that isn't in your inventory.";

        public const string ItemNotOwnedInstruction = "Choose an item from your inventory.";

        public const string NotEnoughSpaceMessage = "You don't have enough space in your inventory.";

        public const string NotEnoughSpaceInstruction = "Upgrade your inventory or free up some space by selling something you don't need.";

        public const string MaximumUpgradeReachedMessage = "You've reached the maximum possible upgrade of the inventory.";

        public const string MaximumUpgradeReachedInstruction = "You cannot upgrade futher.";
    }
}
