using Domain.Ingradients;
using Domain.Recipes;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            // Встановлюємо первинний ключ
            builder.HasKey(i => i.Id);

            // Визначення перетворення для ідентифікатора
            var ingredientIdConverter = new IngredientIdConverter();

            builder.Property(i => i.Id)
                .HasConversion(ingredientIdConverter); // Перетворення для IngredientId

            // Налаштування зв'язку з RecipeIngredient
            builder.HasMany(i => i.RecipeIngredients) // Ingredient може мати багато RecipeIngredients
                .WithOne(ri => ri.Ingredient) // Один Ingredient в RecipeIngredient
                .HasForeignKey(ri => ri.IngredientId); // Зв'язок через IngredientId
        }
    }
}