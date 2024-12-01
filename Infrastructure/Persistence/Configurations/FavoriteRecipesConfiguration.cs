using Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FavoriteRecipesConfiguration : IEntityTypeConfiguration<FavoriteRecipes>
{
    public void Configure(EntityTypeBuilder<FavoriteRecipes> builder)
    {
        builder.HasKey(fr => fr.Id);
        
        // Замість перетворення через HasConversion використовується Guid напряму.
        builder.Property(fr => fr.UserId).IsRequired();
        builder.Property(fr => fr.RecipeId).IsRequired();

        builder.HasOne(fr => fr.UserR)
            .WithMany(u => u.FavoriteRecipes)
            .HasForeignKey(fr => fr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fr => fr.Recipe)
            .WithMany(r => r.FavoriteRecipes)
            .HasForeignKey(fr => fr.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}