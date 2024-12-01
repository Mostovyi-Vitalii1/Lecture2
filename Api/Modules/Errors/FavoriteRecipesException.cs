using System;
using Api.Modules.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors

{
    public abstract class FavoriteRecipesException : Exception
    {
        protected FavoriteRecipesException(string message) : base(message) { }

        protected FavoriteRecipesException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class FavoriteRecipeNotFoundException : FavoriteRecipesException
    {
        public FavoriteRecipeNotFoundException(Guid recipeId)
            : base($"Favorite recipe with ID '{recipeId}' was not found.")
        {
        }
    }

    public class FavoriteRecipeAlreadyExistsException : FavoriteRecipesException
    {
        public FavoriteRecipeAlreadyExistsException(Guid userId, Guid recipeId)
            : base($"User with ID '{userId}' already has a favorite recipe with ID '{recipeId}'.")
        {
        }
    }

    public class FavoriteRecipeUnknownException : FavoriteRecipesException
    {
        public FavoriteRecipeUnknownException(Guid recipeId, Exception innerException)
            : base($"An unknown error occurred with the favorite recipe ID '{recipeId}'.", innerException)
        {
        }
    }

}



public static class FavoriteRecipesErrorHandler
{
    public static ObjectResult ToObjectResult(this FavoriteRecipesException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                FavoriteRecipeNotFoundException => StatusCodes.Status404NotFound,
                FavoriteRecipeAlreadyExistsException => StatusCodes.Status409Conflict,
                FavoriteRecipeUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("FavoriteRecipes error handler does not implemented")
            }
        };
    }
}
