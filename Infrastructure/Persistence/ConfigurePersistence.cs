using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRRepository>();
        services.AddScoped<IUserRRepository>(provider => provider.GetRequiredService<UserRRepository>());
        services.AddScoped<IUserRQueries>(provider => provider.GetRequiredService<UserRRepository>());

        services.AddScoped<IngredientRepository>();
        services.AddScoped<IIngredientRepository>(provider => provider.GetRequiredService<IngredientRepository>());
        services.AddScoped<IIngredientQueries>(provider => provider.GetRequiredService<IngredientRepository>());
        
        services.AddScoped<FavoriteRecipesRepository>();
        services.AddScoped<IFavoriteRecipesRepository>(provider => provider.GetRequiredService<FavoriteRecipesRepository>());
        services.AddScoped<IFavoriteRecipesQueries>(provider => provider.GetRequiredService<FavoriteRecipesRepository>());
        
        services.AddScoped<RecipeRepository>();
        services.AddScoped<IRecipeRepository>(provider => provider.GetRequiredService<RecipeRepository>());
        services.AddScoped<IRecipeQueries>(provider => provider.GetRequiredService<RecipeRepository>());
        
        services.AddScoped<RecipeIngredientsRepository>();
        services.AddScoped<IRecipeIngredientsRepository>(provider => provider.GetRequiredService<RecipeIngredientsRepository>());
        services.AddScoped<IRecipeIngredientsQueries>(provider => provider.GetRequiredService<RecipeIngredientsRepository>());
    }
}