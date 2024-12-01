using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Ingradients;
using Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class IngredientRepository : IIngredientRepository, IIngredientQueries
{
    private readonly ApplicationDbContext _context;

    public IngredientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Реалізація GetById
    public async Task<Ingredient?> GetById(IngredientId id, CancellationToken cancellationToken)
    {
        // Пошук інгредієнта в базі даних за його ID
        var ingredient = await _context.Ingredients
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync(cancellationToken);  // Повертається перший знайдений інгредієнт або null, якщо не знайдено

        return ingredient;  // Повертає інгредієнт або null
    }

    // Інші методи репозиторію
    public async Task AddAsync(Ingredient ingredient, CancellationToken cancellationToken)
    {
        await _context.Ingredients.AddAsync(ingredient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Ingredient> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Name == name, cancellationToken);
    }

    async Task<Ingredient?> IIngredientRepository.GetById(IngredientId id, CancellationToken cancellationToken)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }


// Метод для отримання всіх інгредієнтів
    public async Task<IReadOnlyList<Ingredient>> GetAll(CancellationToken cancellationToken)
    {
        // Повертає список всіх інгредієнтів з бази даних
        var ingredients = await _context.Ingredients
            .ToListAsync(cancellationToken);

        return ingredients;
    }
    public async Task<Ingredient> Add(Ingredient ingredient, CancellationToken cancellationToken)
    {
        await _context.Ingredients.AddAsync(ingredient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return ingredient;
    }

    public async Task<Ingredient> Update(Ingredient ingredient, CancellationToken cancellationToken)
    {
        _context.Ingredients.Update(ingredient);
        await _context.SaveChangesAsync(cancellationToken);
        return ingredient;
    }

    public async Task<Ingredient> Delete(Ingredient ingredient, CancellationToken cancellationToken)
    {
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync(cancellationToken);
        return ingredient;
    }
}