using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Recipes;
namespace Infrastructure.Persistence.Repositories
{
    public class RecipeIngredientsRepository : IRecipeIngredientsRepository, IRecipeIngredientsQueries
    {
        private readonly ApplicationDbContext _context;

        public RecipeIngredientsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Методи для запитів
        public async Task<RecipeIngredient> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<RecipeIngredient>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<RecipeIngredient>> GetAllByRecipeId(Guid recipeId, CancellationToken cancellationToken)
        {
            return await _context.Set<RecipeIngredient>()
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync(cancellationToken);
        }



        public async Task<List<RecipeIngredient>> GetAllByIngredientId(Guid ingredientId, CancellationToken cancellationToken)
        {
            return await _context.Set<RecipeIngredient>()
                .Where(ri => ri.IngredientId == ingredientId) // Це тепер працює завдяки перевантаженим операторам
                .ToListAsync(cancellationToken);
        }



        // Методи для операцій CRUD
        public async Task<RecipeIngredient> Add(RecipeIngredient recipeIngredient, CancellationToken cancellationToken)
        {
            _context.Set<RecipeIngredient>().Add(recipeIngredient);
            await _context.SaveChangesAsync(cancellationToken);
            return recipeIngredient;
        }

        public async Task Update(RecipeIngredient recipeIngredient, CancellationToken cancellationToken)
        {
            _context.Set<RecipeIngredient>().Update(recipeIngredient);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(RecipeIngredient recipeIngredient, CancellationToken cancellationToken)
        {
            _context.Set<RecipeIngredient>().Remove(recipeIngredient);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
