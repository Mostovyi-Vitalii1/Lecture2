using Domain.Ingradients;

namespace Domain.Recipes;

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