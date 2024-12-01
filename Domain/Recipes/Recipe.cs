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
        public List<FavoriteRecipes> FavoriteRecipes { get; set; }   
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        // Конструктор, що приймає TimeSpan для PreparationTime
        public Recipe(string name, string description, TimeSpan preparationTime)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            PreparationTime = preparationTime;
            CreatedAt = DateTime.Now;
            RecipeIngredients = new List<RecipeIngredient>();
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
    }

    // Модель для RecipeIngredient
    public class RecipeIngredient
    {
        public RecipeIngredientsId   Id { get; set; }
        public Guid RecipeId { get; set; }
        public IngredientId IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }

        public RecipeIngredient(Guid recipeId, RecipeIngredientsId id, IngredientId ingredientId, string ingredientName, decimal quantity, string unit)
        {
            RecipeId = recipeId;
            Id = id;  // Використовуємо правильний тип для Id
            IngredientId = ingredientId;
            IngredientName = ingredientName;
            Quantity = quantity;
            Unit = unit;
        }

    }
}
