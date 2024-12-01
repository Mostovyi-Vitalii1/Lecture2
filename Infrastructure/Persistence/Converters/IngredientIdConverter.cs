using Domain.Recipes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class IngredientIdConverter : ValueConverter<IngredientId, Guid>
{
    public IngredientIdConverter() 
        : base(id => id.Value, guid => new IngredientId(guid))
    { }
}