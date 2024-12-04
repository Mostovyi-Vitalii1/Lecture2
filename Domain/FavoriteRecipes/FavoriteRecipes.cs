using Domain.Users;
using Domain.Recipes;

namespace Domain.Recipes;

public class FavoriteRecipes
{
    public FavoriteRecipesId Id { get; }  // Використовуємо FavoriteRecipesId
    public UserRId UserId { get; }  // Використовуємо UserRId
    public Guid RecipeId { get; }  // Використовуємо Guid для RecipeId

    public UserR UserR { get; set; }  // Навігаційна властивість
    public Recipe Recipe { get; set; } // Навігаційна властивість

    public FavoriteRecipes(UserRId userId, Guid recipeId)
    {
        Id = FavoriteRecipesId.New();
        UserId = userId;
        RecipeId = recipeId;
    }
    public FavoriteRecipes(FavoriteRecipesId id, UserRId userId, Guid recipeId)
    {
        Id = FavoriteRecipesId.New();
        UserId = userId;
        RecipeId = recipeId;
    }
    
    public static FavoriteRecipes New(FavoriteRecipesId id, UserRId userId, RecipeId recipeId)
    {
        return new FavoriteRecipes(id, userId, recipeId);
    }
}



public record FavoriteRecipesId(Guid Value)
{
    public static FavoriteRecipesId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
        public static implicit operator Guid(FavoriteRecipesId id) => id.Value; // Додати імпліцитне перетворення

}

