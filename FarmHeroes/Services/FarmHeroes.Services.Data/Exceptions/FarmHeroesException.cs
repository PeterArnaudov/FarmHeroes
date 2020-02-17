namespace FarmHeroes.Services.Data.Exceptions
{
    using System;

    public class FarmHeroesException : Exception
    {
        public FarmHeroesException(string message, string instructions, string redirectPath)
            : base(message)
        {
            this.Instructions = instructions;
            this.RedirectPath = redirectPath;
        }

        public string Instructions { get; set; }

        public string RedirectPath { get; set; }
    }
}
