using System;
using System.Collections.Generic;
using Domain.Ingradients;
using Domain.Recipes;

namespace Tests.Data
{
    public static class RecipesData
    {
        public static Recipe DefaultRecipe
        {
            get
            {
                var recipe = new Recipe(
                    "Spaghetti Carbonara", // Назва рецепту
                    "Cook pasta, add eggs and cheese, mix with bacon.", // Опис рецепту
                    TimeSpan.FromMinutes(30) // Час приготування
                );

                // Ініціалізація інгредієнтів з використанням Guid для RecipeIngredient
                var ingredientId1 = Guid.NewGuid(); // Використовуємо Guid
                var ingredientId2 = Guid.NewGuid();
                var ingredientId3 = Guid.NewGuid();
                var ingredientId4 = Guid.NewGuid();

                recipe.AddIngredients(new List<RecipeIngredient>
                {
                    new RecipeIngredient("Spaghetti", 200m, "g", ingredientId1, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Eggs", 2m, "pcs", ingredientId2, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Parmesan Cheese", 100m, "g", ingredientId3, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Bacon", 150m, "g", ingredientId4, new IngredientId(Guid.NewGuid()), recipe)
                });

                return recipe;
            }
        }


        public static Recipe AnotherRecipe
        {
            get
            {
                var recipe = new Recipe(
                    "Pasta Bolognese", // Назва рецепту
                    "Cook pasta, make bolognese sauce with ground beef, mix together.", // Опис рецепту
                    TimeSpan.FromMinutes(45) // Час приготування
                );

                recipe.AddIngredients(new List<RecipeIngredient>
                {
                    new RecipeIngredient("Pasta", 250m, "g", Guid.Empty, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Ground Beef", 300m, "g", Guid.Empty, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Tomato Sauce", 200m, "ml", Guid.Empty, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Onion", 1m, "pcs", Guid.Empty, new IngredientId(Guid.NewGuid()), recipe),
                    new RecipeIngredient("Garlic", 2m, "cloves", Guid.Empty, new IngredientId(Guid.NewGuid()), recipe)
                });

                return recipe;
            }
        }
    }
}
