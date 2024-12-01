using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Ingradients;
using Domain.Recipes;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRecipeIngredientsRepository
    {
        Task<RecipeIngredient> Add(RecipeIngredient recipeIngredient, CancellationToken cancellationToken);
        Task Update(RecipeIngredient recipeIngredient, CancellationToken cancellationToken);
        Task Delete(RecipeIngredient recipeIngredient, CancellationToken cancellationToken);
    }
}