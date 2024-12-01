using Domain.Recipes;

public record FavoriteRecipesDto(
    Guid Id,
    Guid UserId,
    Guid RecipeId,
    string RecipeName)
{
    public static FavoriteRecipesDto FromDomainModel(FavoriteRecipes favoriteRecipe)
        => new(
            Id: favoriteRecipe.Id,  // Автоматично конвертується через імпліцитний оператор
            UserId: favoriteRecipe.UserId, // Автоматично конвертується через імпліцитний оператор
            RecipeId: favoriteRecipe.RecipeId, // Тут вже Guid, тому не потрібне перетворення
            RecipeName: favoriteRecipe.Recipe?.Name ?? string.Empty);
}