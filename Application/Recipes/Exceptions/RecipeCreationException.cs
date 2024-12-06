public class RecipeCreationException : Exception
{
    public RecipeCreationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}