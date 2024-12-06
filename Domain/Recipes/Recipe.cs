using System;
using System.Collections.Generic;

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
        // Використовуємо private setter для колекції

        // Конструктор, що приймає TimeSpan для PreparationTime
        public Recipe(string name, string description, TimeSpan preparationTime)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            PreparationTime = preparationTime;
            CreatedAt = DateTime.UtcNow;
            RecipeIngredients = new List<RecipeIngredient>(); // ініціалізація списку інгредієнтів
        }

        // Конструктор для ініціалізації основних властивостей
        public Recipe(string name, string description = null)
        {
            Name = name;
            Description = description;
            RecipeIngredients = new List<RecipeIngredient>(); // ініціалізація списку інгредієнтів
        }
        public Recipe()
        {
            RecipeIngredients = new List<RecipeIngredient>();
        }
        // Оновлений метод для створення нового рецепту
        public static Recipe New(string name, string description, int preparationTimeMinutes,
            List<RecipeIngredient> recipeIngredients)
        {
            var preparationTime = TimeSpan.FromMinutes(preparationTimeMinutes); // Конвертуємо в TimeSpan
            var recipe = new Recipe(name, description, preparationTime);
            recipe.AddIngredients(recipeIngredients); // Додаємо інгредієнти
            return recipe;
        }

        // Оновлений метод для оновлення Recipe
        public void UpdateDetails(string name, string description, int preparationTimeMinutes,
            List<RecipeIngredient> recipeIngredients)
        {
            Name = name;
            Description = description;
            PreparationTime = TimeSpan.FromMinutes(preparationTimeMinutes); // Конвертуємо int в TimeSpan
            UpdateIngredients(recipeIngredients); // Оновлюємо інгредієнти
        }
        
        // Оновлений метод для оновлення Recipe
        public void UpdateDetails(string name, string description, int preparationTimeMinutes)
        {
            Name = name;
            Description = description;
            PreparationTime = TimeSpan.FromMinutes(preparationTimeMinutes); // Конвертуємо int в TimeSpan
        }

        // Метод для додавання інгредієнтів
        public void AddIngredients(List<RecipeIngredient> recipeIngredients)
        {
            if (recipeIngredients != null)
            {
                foreach (var ingredient in recipeIngredients)
                {
                    RecipeIngredients.Add(ingredient); // Додаємо кожен інгредієнт по черзі
                }
            }
        }
        public void AddIngredient(RecipeIngredient ingredient)
        {
            // Додавання інгредієнта до списку інгредієнтів
            RecipeIngredients.Add(ingredient);
        }

        // Метод для оновлення інгредієнтів
        public void UpdateIngredients(List<RecipeIngredient> recipeIngredients)
        {
            if (recipeIngredients != null)
            {
                RecipeIngredients.Clear(); // Очищаємо старі інгредієнти
                foreach (var ingredient in recipeIngredients)
                {
                    RecipeIngredients.Add(ingredient); // Додаємо кожен новий інгредієнт по черзі
                }
            }
        }
    }
}
