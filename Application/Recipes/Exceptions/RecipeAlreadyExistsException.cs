using Domain.Recipes;

namespace Application.Recipes.Exceptions
{
    public class RecipeAlreadyExistsException : RecipeException
    {
        public RecipeAlreadyExistsException(RecipeId recipeId)
            : base($"Recipe with ID '{recipeId}' already exists.")
        {
        }
    }

    public class RecipeUnknownException : RecipeException
    {
        public RecipeUnknownException(RecipeId recipeId, Exception innerException)
            : base($"Unknown error occurred for recipe with ID '{recipeId}'.", innerException)
        {
        }
    }

    public abstract class RecipeException : Exception
    {
        protected RecipeException(string message) : base(message) { }
        protected RecipeException(string message, Exception innerException) : base(message, innerException) { }
    }
}