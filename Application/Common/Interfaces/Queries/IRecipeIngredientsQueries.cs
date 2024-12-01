using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Ingradients;
using Domain.Recipes;

namespace Application.Common.Interfaces.Queries
{
    public interface IRecipeIngredientsQueries
    {
        Task<RecipeIngredient> GetById(Guid id, CancellationToken cancellationToken);
        Task<List<RecipeIngredient>> GetAllByRecipeId(Guid recipeId, CancellationToken cancellationToken);
        Task<List<RecipeIngredient>> GetAllByIngredientId(Guid ingredientId, CancellationToken cancellationToken);
    }
}