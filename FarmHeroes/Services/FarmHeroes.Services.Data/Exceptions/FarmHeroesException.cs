namespace FarmHeroes.Services.Data.Exceptions
{
    using System;

    /// <summary>
    /// Custom exception tailored to fulfill the needs of the application.
    /// </summary>
    public class FarmHeroesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FarmHeroesException"/> class with a specified message, instructions and redirect path.
        /// </summary>
        /// <param name="message">
        /// The message that describes what's gone wrong.
        /// </param>
        /// <param name="instructions">
        /// Instructions to help the user.
        /// </param>
        /// <param name="redirectPath">
        /// A path where to redirect the user upon thrown <see cref="FarmHeroesException"/>.
        /// </param>
        public FarmHeroesException(string message, string instructions, string redirectPath)
            : base(message)
        {
            this.Instructions = instructions;
            this.RedirectPath = redirectPath;
        }

        /// <summary>
        /// Gets or sets a message that instructs the user.
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Gets or sets a redirect path used by <see cref="FarmHeroes.Web.Filters.FarmHeroesExceptionFilterAttribute"/> to redirect the user.
        /// </summary>
        public string RedirectPath { get; set; }
    }
}
