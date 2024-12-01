using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
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
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Recipe> Add(Recipe recipe, CancellationToken cancellationToken)
    {
        await _context.Recipes.AddAsync(recipe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }

    public async Task<Recipe> Update(Recipe recipe, CancellationToken cancellationToken)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }

    public async Task<Recipe> Delete(Recipe recipe, CancellationToken cancellationToken)
    {
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }
}