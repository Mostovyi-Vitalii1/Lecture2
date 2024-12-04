using System;
using System.Collections.Generic;
using Domain.Ingradients;

namespace Domain.Recipes
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<FavoriteRecipes> FavoriteRecipes { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        // Конструктор, що приймає TimeSpan для PreparationTime
        public Recipe(string name, string description, TimeSpan preparationTime)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            PreparationTime = preparationTime;
            CreatedAt = DateTime.UtcNow;
            RecipeIngredients = new List<RecipeIngredient>();
        }
        // Конструктор для ініціалізації основних властивостей
        public Recipe(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        // Оновлений метод New для роботи з int для preparationTime
        public static Recipe New(string name, string description, int preparationTimeMinutes,
            List<RecipeIngredient> recipeIngredients)
        {
            var preparationTime = TimeSpan.FromMinutes(preparationTimeMinutes);  // Конвертуємо int в TimeSpan
            var recipe = new Recipe(name, description, preparationTime); // Викликаємо конструктор з TimeSpan
            recipe.RecipeIngredients = recipeIngredients;
            return recipe;
        }

        // Оновлений метод для оновлення Recipe
        public void UpdateDetails(string name, string description, int preparationTimeMinutes,
            List<RecipeIngredient> recipeIngredients)
        {
            Name = name;
            Description = description;
            PreparationTime = TimeSpan.FromMinutes(preparationTimeMinutes); // Конвертуємо int в TimeSpan
            RecipeIngredients = recipeIngredients;
        }

        public static Recipe New(RecipeId name, string mainRecipeTitle, string preparationTimeMinutes, DateTime recipeIngredients)
        {
            name = name;
            mainRecipeTitle = mainRecipeTitle;
            preparationTimeMinutes = preparationTimeMinutes;
            return new Recipe(mainRecipeTitle, preparationTimeMinutes);
        }
    }

 
}
