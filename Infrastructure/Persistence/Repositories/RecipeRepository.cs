using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
using Domain.Ingradients;
using Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class RecipeRepository : IRecipeRepository, IRecipeQueries
{
    private readonly ApplicationDbContext _context;

    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Option<Recipe>> GetById(RecipeId id, CancellationToken cancellationToken)
    {
        var entity = await _context.Recipes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken); // Порівняння Guid значень

        return entity == null ? Option.None<Recipe>() : Option.Some(entity);
    }


    public async Task<IReadOnlyList<Recipe>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients) // Завантаження інгредієнтів
            .ThenInclude(ri => ri.Ingredient) // Якщо потрібні деталі інгредієнтів
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveAllIngredientsFromRecipe(Guid recipeId, CancellationToken cancellationToken)
    {
        var recipe = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);
        
        if (recipe != null)
        {
            _context.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task UpdateIngredientsForRecipe(Guid recipeId, List<RecipeIngredient> newIngredients, CancellationToken cancellationToken)
    {
        var recipe = await _context.Recipes
            .Include(r => r.RecipeIngredients) // Завантажуємо інгредієнти
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);

        if (recipe == null)
        {
            throw new KeyNotFoundException($"Recipe with ID {recipeId} not found.");
        }

        // Оновлюємо або додаємо нові інгредієнти
        foreach (var ri in newIngredients)
        {
            var existingIngredient = recipe.RecipeIngredients
                .FirstOrDefault(ri => ri.IngredientId == ri.IngredientId);

            if (existingIngredient != null)
            {
                // Якщо інгредієнт вже є, оновлюємо його кількість та одиницю виміру
                existingIngredient.Quantity = ri.Quantity;
                existingIngredient.Unit = ri.Unit;
            }
            else
            {
                // Додаємо новий інгредієнт, якщо його ще немає
                recipe.RecipeIngredients.Add(ri);
            }
        }

        // Збереження змін в базі даних
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddIngredientToRecipe(Guid recipeId, RecipeIngredient ingredient, CancellationToken cancellationToken)
    {
        // Завантажуємо рецепт за ID
        var recipe = await _context.Recipes
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);

        if (recipe != null)
        {
            // Перевіряємо, чи інгредієнт вже доданий до рецепту
            var existingIngredient = recipe.RecipeIngredients
                .FirstOrDefault(ri => ri.IngredientId == ingredient.IngredientId);

            if (existingIngredient == null)
            {
                // Додаємо новий інгредієнт
                recipe.RecipeIngredients.Add(ingredient);
            
                try
                {
                    // Зберігаємо зміни в базі
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    // Логування або інша обробка помилок
                    throw new Exception("Error saving changes to the database.", ex);
                }
            }
            else
            {
                // Якщо інгредієнт вже є, можна оновити його кількість або інші властивості
                existingIngredient.Quantity = ingredient.Quantity;
                existingIngredient.Unit = ingredient.Unit;

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating ingredient in the recipe.", ex);
                }
            }
        }
        else
        {
            throw new KeyNotFoundException($"Recipe with ID {recipeId} not found.");
        }
        
    }




    public async Task<Recipe> Add(Recipe recipe, CancellationToken cancellationToken)
    {
        await _context.Recipes.AddAsync(recipe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }

    public async Task<Recipe> Update(Recipe recipe, CancellationToken cancellationToken)
    {
        var existingRecipe = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id, cancellationToken);

        if (existingRecipe == null)
        {
            throw new Exception("Recipe not found");
        }

        // Оновлення основних властивостей
        existingRecipe.Name = recipe.Name;
        existingRecipe.Description = recipe.Description;
        existingRecipe.PreparationTime = recipe.PreparationTime;

        // Оновлення інгредієнтів
        _context.RecipeIngredients.RemoveRange(existingRecipe.RecipeIngredients);
        _context.RecipeIngredients.AddRange(recipe.RecipeIngredients);

        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }

    public async Task<Ingredient> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Name == name, cancellationToken);
    }

    public async Task<Recipe> Delete(Recipe recipe, CancellationToken cancellationToken)
    {
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }
}