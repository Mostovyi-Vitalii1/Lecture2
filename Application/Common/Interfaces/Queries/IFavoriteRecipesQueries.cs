using Domain.Recipes;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IFavoriteRecipesQueries
{
    Task<IReadOnlyList<FavoriteRecipes>> GetAllByUserId(UserRId userId, CancellationToken cancellationToken);
    Task<Option<FavoriteRecipes>> GetByUserIdAndRecipeId(UserRId userId, RecipeId recipeId, CancellationToken cancellationToken);
}