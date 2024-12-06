using System.Reflection;
using Domain.Recipes;
using Domain.Ingradients;
using Domain.Users;
using Infrastructure.Persistence.Configurations;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<UserR> UsersR { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<FavoriteRecipes> FavoriteRecipes { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        var favoriteRecipesIdConverter = new ValueConverter<FavoriteRecipesId, Guid>(
            id => id.Value, 
            guid => new FavoriteRecipesId(guid));

        builder.Entity<FavoriteRecipes>()
            .Property(fr => fr.Id)
            .HasConversion(favoriteRecipesIdConverter);
        
        
        base.OnModelCreating(builder);
    }
}
