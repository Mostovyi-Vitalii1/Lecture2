namespace Domain.Recipes;

// Ідентифікатор рецепту
public record RecipeId(Guid Value)
{
    public static RecipeId New() => new(Guid.NewGuid());
    public static RecipeId Empty() => new(Guid.Empty);

    public override string ToString() => Value.ToString();
    
    
    // Оператор явного перетворення в Guid?
    public static explicit operator Guid?(RecipeId recipeId) => recipeId.Value == Guid.Empty ? null : recipeId.Value;
    public static implicit operator Guid(RecipeId id) => id.Value;
    // Оператор неявного перетворення з Guid? в UserRId
    public static implicit operator RecipeId(Guid? value) => new(value ?? Guid.Empty);
}

