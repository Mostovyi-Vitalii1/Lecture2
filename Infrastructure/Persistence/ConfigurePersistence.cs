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
        // Налаштування підключення до PostgreSQL через NpgsqlDataSource
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();  // Підтримка динамічного JSON в PostgreSQL
        var dataSource = dataSourceBuild.Build();

        // Додавання ApplicationDbContext до DI
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dataSource, builder =>
                builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention() // Використання зміщення імен в стилі snake_case
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))
        );

        // Ініціалізація і реєстрація ApplicationDbContextInitialiser
        services.AddScoped<ApplicationDbContextInitialiser>();

        // Додавання репозиторіїв та запитів через AddRepositories
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // Реєстрація репозиторіїв та їх інтерфейсів
        services.AddScoped<IUserRRepository, UserRRepository>();
        services.AddScoped<IUserRQueries, UserRRepository>();

        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IIngredientQueries, IngredientRepository>();

        services.AddScoped<IFavoriteRecipesRepository, FavoriteRecipesRepository>();
        services.AddScoped<IFavoriteRecipesQueries, FavoriteRecipesRepository>();

        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IRecipeQueries, RecipeRepository>();

        services.AddScoped<IRecipeIngredientsRepository, RecipeIngredientsRepository>();
        services.AddScoped<IRecipeIngredientsQueries, RecipeIngredientsRepository>();
    }
}
