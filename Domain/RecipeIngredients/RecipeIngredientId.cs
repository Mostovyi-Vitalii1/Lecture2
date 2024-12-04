namespace Domain.Recipes;


// Ідентифікатор для RecipeIngredient
public record RecipeIngredientsId (Guid Value)
{
    public static RecipeIngredientsId New() => new(Guid.NewGuid());
    public static IngredientId Empty() => new(Guid.Empty);
        
    public override string ToString() => Value.ToString();

}