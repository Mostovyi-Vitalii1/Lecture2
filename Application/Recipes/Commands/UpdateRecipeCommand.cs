using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using CSharpFunctionalExtensions;
using Domain.Ingradients;
using Domain.Recipes;
using MediatR;
using Optional.Unsafe;

namespace Application.Recipes.Commands
{
    public class RecipeIngredientUpdateModel
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        public RecipeIngredientUpdateModel(string name, decimal quantity, string unit)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
        }
    }

    public class UpdateRecipeCommand : IRequest<Result>
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan PreparationTimeMinutes { get; set; }

        public List<RecipeIngredient> Ingredients { get; set; }

        public UpdateRecipeCommand(Guid recipeId, string name, string description, TimeSpan preparationTimeMinutes, List<RecipeIngredient> ingredients)
        {
            RecipeId = recipeId;
            Name = name;
            Description = description;
            PreparationTimeMinutes = preparationTimeMinutes;
            Ingredients = ingredients;
        }
    }

    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Result>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Result> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            // Отримуємо рецепт за ID
            var recipeOption = await _recipeRepository.GetById(new RecipeId(request.RecipeId), cancellationToken);

            if (!recipeOption.HasValue)
            {
                return Result.Failure("Recipe not found.");
            }

            var recipe = recipeOption.ValueOrFailure();

            // Оновлюємо властивості рецепту
            recipe.Name = request.Name;
            recipe.Description = request.Description;
            recipe.PreparationTime = request.PreparationTimeMinutes;

            // Оновлюємо інгредієнти
            foreach (var ri in request.Ingredients)
            {
                var existingRecipeIngredient = recipe.RecipeIngredients
                    .FirstOrDefault(r => r.IngredientId == ri.IngredientId);

                if (existingRecipeIngredient != null)
                {
                    // Оновлюємо кількість та одиницю виміру інгредієнта
                    existingRecipeIngredient.Update(ri.Quantity, ri.Unit);
                }
                else
                {
                    // Якщо такого інгредієнта не було, додаємо новий
                    recipe.AddIngredient(new RecipeIngredient(
                        recipe.Id,
                        RecipeIngredientsId.New(),
                        ri.IngredientId,
                        ri.IngredientName,
                        ri.Quantity,
                        ri.Unit
                    ));
                }
            }

            // Зберігаємо зміни
            await _recipeRepository.Update(recipe, cancellationToken);  // Переконайтеся, що метод UpdateAsync існує

            return Result.Success();
        }
    }
}
