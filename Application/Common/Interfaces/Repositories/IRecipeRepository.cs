using Domain.Recipes;
using Optional;

public interface IRecipeRepository
{
    Task<Recipe> Add(Recipe recipe, CancellationToken cancellationToken);
    Task<Recipe> Update(Recipe recipe, CancellationToken cancellationToken);
    Task<Recipe> Delete(Recipe recipe, CancellationToken cancellationToken);
    Task<Option<Recipe>> GetById(RecipeId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Recipe>> GetAll(CancellationToken cancellationToken);
    Task RemoveAllIngredientsFromRecipe(Guid recipeId, CancellationToken cancellationToken);
    Task AddIngredientToRecipe(Guid recipeId, RecipeIngredient ingredient, CancellationToken cancellationToken);
    Task UpdateIngredientsForRecipe(Guid recipeId, List<RecipeIngredient> newIngredients, CancellationToken cancellationToken);

}