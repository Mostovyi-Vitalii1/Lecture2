using Domain.Recipes;
using Domain.Ingradients;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations
{
    public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            // Встановлюємо первинний ключ
            builder.HasKey(ri => ri.Id); 
            builder.ToTable("RecipeIngredients");

            // Визначення перетворення для ідентифікаторів
            var recipeIngredientsIdConverter = new RecipeIngredientsIdConverter();
            var ingredientIdConverter = new IngredientIdConverter();

            builder.Property(ri => ri.Id)
                .HasConversion(recipeIngredientsIdConverter); // Перетворення для RecipeIngredientsId

            builder.Property(ri => ri.IngredientId)
                .HasConversion(ingredientIdConverter) // Перетворення для IngredientId
                .IsRequired(); // Зовнішній ключ, обов'язковий

            // Встановлюємо властивість для IngredientName (можна додати довжину або інші налаштування)
            builder.Property(ri => ri.IngredientName)
                .IsRequired()
                .HasMaxLength(255); // Наприклад, обмеження на довжину

            // Встановлення значення за замовчуванням для Quantity (якщо потрібно)
            builder.Property(ri => ri.Quantity)
                .HasDefaultValue(0); // Якщо значення не задано, буде 0

            // Встановлюємо властивість для Unit
            builder.Property(ri => ri.Unit)
                .IsRequired()
                .HasMaxLength(50); // Можна обмежити довжину юніту

            // Налаштування зв'язку з Ingredient
            builder.HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)  // Явно вказано зовнішній ключ
                .OnDelete(DeleteBehavior.Cascade);

            // Налаштування зв'язку з Recipe
            builder.HasOne(ri => ri.Recipe) // Один Recipe для кожного RecipeIngredient
                .WithMany(r => r.RecipeIngredients) // Recipe може мати багато RecipeIngredients
                .HasForeignKey(ri => ri.RecipeId) // Зв'язок через RecipeId
                .OnDelete(DeleteBehavior.Cascade); // Видалення залежних записів
        }
    }
}
