namespace Domain.Recipes
{
    public record IngredientId(Guid Value)
    {
        public static IngredientId New() => new(Guid.NewGuid());
        public static IngredientId Empty() => new(Guid.Empty);
        
        public override string ToString() => Value.ToString();

        // Оператор порівняння IngredientId з Guid
        public static bool operator ==(IngredientId left, Guid right)
        {
            return left?.Value == right;
        }

        public static bool operator !=(IngredientId left, Guid right)
        {
            return left?.Value != right;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}