using Api.Dtos;
using Domain.Recipes;

public record RecipeDto(
    Guid Id,
    
    string Name,
    string Description,
    int PreparationTimeMinutes,
    DateTime CreatedAt,
    List<IngredientRequestDto> Ingredients)
{
    public static RecipeDto FromDomainModel(Recipe recipe) => new(
        Id: recipe.Id,
        Name: recipe.Name,
        Description: recipe.Description,
        PreparationTimeMinutes: (int)recipe.PreparationTime.TotalMinutes,
        CreatedAt: recipe.CreatedAt,
        Ingredients: recipe.RecipeIngredients
            .Select(ri => new IngredientRequestDto(
                ri.IngredientName, // Назва інгредієнта
                ri.Quantity,       // Кількість
                ri.Unit))          // Одиниця вимірювання
            .ToList());

}

/*
public record CreateRecipeDto(
    string Name,
    string Description,
    int PreparationTimeMinutes,
    DateTime CreatedAt,
    List<IngredientRequestDto> Ingredients);
    */
