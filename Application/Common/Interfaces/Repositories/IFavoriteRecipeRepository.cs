using Domain.Recipes;
using Domain.Recipes;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IFavoriteRecipesRepository
{
    Task<FavoriteRecipes> Add(FavoriteRecipes favoriteRecipe, CancellationToken cancellationToken);
    Task<FavoriteRecipes> Update(FavoriteRecipes favoriteRecipe, CancellationToken cancellationToken);
    Task<FavoriteRecipes> Delete(FavoriteRecipes favoriteRecipe, CancellationToken cancellationToken);
    Task<Option<FavoriteRecipes>> GetById(FavoriteRecipesId id, CancellationToken cancellationToken);
    Task<Option<FavoriteRecipes>> GetByUserIdAndRecipeId(UserRId userId, RecipeId recipeId, CancellationToken cancellationToken);
}