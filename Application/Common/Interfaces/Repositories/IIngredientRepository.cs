using Domain.Ingradients;
using Domain.Recipes;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IIngredientRepository
{
    Task<Ingredient> Add(Ingredient ingredient, CancellationToken cancellationToken);
    Task<Ingredient> Update(Ingredient ingredient, CancellationToken cancellationToken);
    Task<Ingredient> Delete(Ingredient ingredient, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ingredient>> GetAll(CancellationToken cancellationToken);
    Task<Ingredient> GetByNameAsync(string name, CancellationToken cancellationToken); // асинхронний метод для перевірки наявності інгредієнта за назвою
    Task<Ingredient?> GetById(IngredientId id, CancellationToken cancellationToken);  // Повертає Ingredient або null
    Task AddAsync(Ingredient ingredient, CancellationToken cancellationToken); // Асинхронне додавання інгредієнта
}
