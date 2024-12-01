using Domain.Recipes;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> Add(Recipe recipe, CancellationToken cancellationToken);
    Task<Recipe> Update(Recipe recipe, CancellationToken cancellationToken);
    Task<Recipe> Delete(Recipe recipe, CancellationToken cancellationToken);
    Task<Option<Recipe>> GetById(RecipeId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Recipe>> GetAll(CancellationToken cancellationToken);
}