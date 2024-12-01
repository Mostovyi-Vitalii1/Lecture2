using Domain.Recipes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class RecipeIngredientsIdConverter : ValueConverter<RecipeIngredientsId, Guid>
{
    public RecipeIngredientsIdConverter() 
        : base(id => id.Value, value => new RecipeIngredientsId(value)) { }
}