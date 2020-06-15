namespace FarmHeroes.Services.Data.Constants.ExceptionMessages
{
    using System;

    public class DungeonExceptionMessages
    {
        public const string NoMonstersOnFloorMessage = "There are no more monsters on the floor.";

        public const string NoMonstersOnFloorInstruction = "Continue with the next floor or exit the dungeon.";

        public const string HeroCannotStartDungeonMessage = "You cannot enter the dungeon.";

        public const string HeroCannotStartDungeonInstruction = "You should wait until you can enter the dungeon again.";

        public const string HeroAlreadyInDungeonMessage = "You are already in the dungeon.";

        public const string HeroAlreadyInDungeonInstruction = "Keep going deeper in the dungeon or exit to start over.";

        public const string HeroNotInDungeonMessage = "To do this, you have to be in the dungeon.";

        public const string HeroNotInDungeonInstruction = "Enter the dungeon.";
    }
}
