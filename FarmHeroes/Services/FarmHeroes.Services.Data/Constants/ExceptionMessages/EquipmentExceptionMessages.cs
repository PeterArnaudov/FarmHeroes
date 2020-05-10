namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;
    using System.Security.Permissions;

    public class EquipmentExceptionMessages
    {
        public const string DoesNotBelongToHeroMessage = "You cannot equip an item that isn't yours.";

        public const string DoesNotBelongToHeroInstruction = "Go buy yourself items from the shop.";
    }
}
