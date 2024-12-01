using Domain.Recipes;

namespace Application.Ingradients.Exceptions
{
    public class IngredientAlreadyExistsException : IngredientException
    {
        public IngredientAlreadyExistsException(IngredientId ingredientId)
            : base($"Ingredient with ID '{ingredientId}' already exists.")
        {
        }
    }

    public class IngredientUnknownException : IngredientException
    {
        public IngredientUnknownException(IngredientId ingredientId, Exception innerException)
            : base($"Unknown error occurred for ingredient with ID '{ingredientId}'.", innerException)
        {
        }
    }

    public abstract class IngredientException : Exception
    {
        protected IngredientException(string message) : base(message) { }
        protected IngredientException(string message, Exception innerException) : base(message, innerException) { }
    }
}