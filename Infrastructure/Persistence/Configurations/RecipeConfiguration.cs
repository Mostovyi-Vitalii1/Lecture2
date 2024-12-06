using Domain.Recipes;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        // Визначення первинного ключа для Recipe
        builder.HasKey(r => r.Id);

        // Конфігурація властивості Name
        builder.Property(r => r.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");

        // Конфігурація RecipeIngredients як List<RecipeIngredient>
        builder.HasMany(r => r.RecipeIngredients)
            .WithOne(ri => ri.Recipe)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Конфігурація FavoriteRecipes як List<FavoriteRecipes>
        builder.HasMany(r => r.FavoriteRecipes)
            .WithOne(fr => fr.Recipe)
            .HasForeignKey(fr => fr.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
