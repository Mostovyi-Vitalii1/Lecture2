namespace Application.Common.Interfaces.Queries;

using Domain.Recipes;
using Optional;

public interface IRecipeQueries
{
    Task<IReadOnlyList<Recipe>> GetAll(CancellationToken cancellationToken);
    Task<Option<Recipe>> GetById(RecipeId id, CancellationToken cancellationToken);
}
