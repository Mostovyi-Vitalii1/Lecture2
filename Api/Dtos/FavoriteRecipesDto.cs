using Domain.Recipes;

public record FavoriteRecipesDto(
    Guid Id,
    Guid UserId,
    Guid  RecipeId)
{
    
    public static FavoriteRecipesDto FromDomainModel(FavoriteRecipes favoriteRecipe)
        => new(
            Id: favoriteRecipe.Id,  // Автоматично конвертується через імпліцитний оператор
            UserId: favoriteRecipe.UserId, // Автоматично конвертується через імпліцитний оператор
            RecipeId: favoriteRecipe.RecipeId);
    
    
}