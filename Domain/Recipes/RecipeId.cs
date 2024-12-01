namespace Domain.Recipes;

// Ідентифікатор рецепту
public record RecipeId(Guid Value)
{
    public static RecipeId New() => new(Guid.NewGuid());
    public static RecipeId Empty() => new(Guid.Empty);

    public override string ToString() => Value.ToString();
}


// Ідентифікатор для RecipeIngredient
public record RecipeIngredientsId (Guid Value)
{
    public static IngredientId New() => new(Guid.NewGuid());
    public static IngredientId Empty() => new(Guid.Empty);
        
    public override string ToString() => Value.ToString();

}