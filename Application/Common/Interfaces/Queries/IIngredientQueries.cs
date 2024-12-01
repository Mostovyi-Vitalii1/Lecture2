using Domain.Ingradients;
using Domain.Recipes;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IIngredientQueries
{
    Task<IReadOnlyList<Ingredient>> GetAll(CancellationToken cancellationToken);
    Task<Ingredient?> GetById(IngredientId id, CancellationToken cancellationToken);
}