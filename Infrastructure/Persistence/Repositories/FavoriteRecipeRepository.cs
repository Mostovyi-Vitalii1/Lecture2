using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Recipes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class FavoriteRecipesRepository : IFavoriteRecipesRepository, IFavoriteRecipesQueries
{
    private readonly ApplicationDbContext _context;

    public FavoriteRecipesRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<FavoriteRecipes>> GetAllByUserId(UserRId userId, CancellationToken cancellationToken)
    {
        return await _context.FavoriteRecipes
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<FavoriteRecipes> Add(FavoriteRecipes favoriteRecipe, CancellationToken cancellationToken)
    {
        await _context.FavoriteRecipes.AddAsync(favoriteRecipe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return favoriteRecipe;
    }

    public async Task<FavoriteRecipes> Update(FavoriteRecipes favoriteRecipe, CancellationToken cancellationToken)
    {
        _context.FavoriteRecipes.Update(favoriteRecipe);
        await _context.SaveChangesAsync(cancellationToken);
        return favoriteRecipe;
    }

    public async Task<FavoriteRecipes> Delete(FavoriteRecipes favoriteRecipe, CancellationToken cancellationToken)
    {
        _context.FavoriteRecipes.Remove(favoriteRecipe);
        await _context.SaveChangesAsync(cancellationToken);
        return favoriteRecipe;
    }

    public async Task<Option<FavoriteRecipes>> GetById(FavoriteRecipesId id, CancellationToken cancellationToken)
    {
        var entity = await _context.FavoriteRecipes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Value == id.Value, cancellationToken); // Використовуємо x.Id.Value та id.Value

        return entity == null ? Option.None<FavoriteRecipes>() : Option.Some(entity);
    }

    public async Task<Option<FavoriteRecipes>> GetByUserIdAndRecipeId(UserRId userId, RecipeId recipeId, CancellationToken cancellationToken)
    {
        var entity = await _context.FavoriteRecipes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.RecipeId.Equals(recipeId), cancellationToken);

        return entity == null ? Option.None<FavoriteRecipes>() : Option.Some(entity);
    }

}
