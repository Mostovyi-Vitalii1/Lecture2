using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Ingradients;
using Domain.Recipes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.Integration.Recipes;

public class RecipesControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Recipe _defaultRecipe;

    public RecipesControllerTests(IntegrationTestWebFactory factory, ITestOutputHelper testOutputHelper) : base(factory)
    {
        _testOutputHelper = testOutputHelper;
        _defaultRecipe = RecipesData.DefaultRecipe;
    }

    [Fact]
    public async Task ShouldCreateRecipe()
    {
        // Arrange
        var guid = new Guid("e275d55f-a3b0-4736-a791-9a4d6290fb27"); // Використовуємо сталий ідентифікатор
        var recipeName = "Spaghetti Carbonara";
        var request = new RecipeDto(
            Id: guid,  // Використовуємо сталий ID
            Name: recipeName,
            Description: "Cook pasta, add eggs and cheese, mix with bacon.",
            PreparationTimeMinutes: 30,
            CreatedAt: DateTime.UtcNow,
            Ingredients: new List<IngredientRequestDto> 
            { 
                new IngredientRequestDto("Pasta", 200, "grams"),
                new IngredientRequestDto("Eggs", 3, "pieces"),
                new IngredientRequestDto("Cheese", 100, "grams"),
                new IngredientRequestDto("Bacon", 150, "grams")
            });

        // Act
        var response = await Client.PostAsJsonAsync("recipes", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseRecipe = await response.ToResponseModel<RecipeDto>();
        var recipeId = responseRecipe.Id;

        var dbRecipe = await Context.Recipes.Include(r => r.RecipeIngredients).FirstAsync(x => x.Id == recipeId);

        // Перевіряємо, що інгредієнти зберігаються правильно
        dbRecipe.RecipeIngredients.Should().HaveCount(4);
        dbRecipe.RecipeIngredients.Select(ri => ri.IngredientName).Should().Contain("Pasta", "Eggs", "Cheese", "Bacon");
    }

    [Fact]
    public async Task ShouldUpdateRecipe()
    {
        // Arrange
        var request = new RecipeDto(
            Id: Guid.Parse("e275d55f-a3b0-4736-a791-9a4d6290fb27"), // Використовуємо правильний ID
            Name: "string1",
            Description: "string",
            PreparationTimeMinutes: 0,
            CreatedAt: DateTime.Parse("2024-12-06T07:37:14.567Z"), // Задаємо правильну дату
            Ingredients: new List<IngredientRequestDto> 
            { 
                new IngredientRequestDto("string1", 0, "string") // Інгредієнт з правильними даними
            });

        // Act
        var response = await Client.PutAsJsonAsync($"recipes/update/{request.Id}", request);

        // Log the response status code for debugging
        _testOutputHelper.WriteLine($"Status Code: {response.StatusCode}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent); // Перевірка на код 204

        var dbRecipe = await Context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        // Перевіряємо, що назва, опис та час приготування змінені
        dbRecipe?.Name.Should().Be(request.Name);
        dbRecipe?.Description.Should().Be(request.Description);
        dbRecipe?.PreparationTime.TotalMinutes.Should().Be(request.PreparationTimeMinutes);

        // Перевіряємо, що інгредієнти були оновлені
        dbRecipe?.RecipeIngredients.Count.Should().Be(request.Ingredients.Count);

        // Перевіряємо, чи всі інгредієнти зберігаються
        dbRecipe?.RecipeIngredients.Select(ri => ri.IngredientName).Should().Contain("string1");
    }


    [Fact]
    public async Task ShouldDeleteRecipe()
    {
        // Arrange
        var recipeId = new Guid("e275d55f-a3b0-4736-a791-9a4d6290fb27"); // Використовуємо сталий ID для видалення

        // Act
        var response = await Client.DeleteAsync($"recipes/{recipeId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbRecipe = await Context.Recipes.FirstOrDefaultAsync(x => x.Id == recipeId);
        dbRecipe.Should().BeNull();
    }

    [Fact]
    public async Task ShouldGetAllRecipes()
    {
        // Act
        var response = await Client.GetAsync("recipes");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var recipes = await response.ToResponseModel<List<RecipeDto>>();
        recipes.Should().ContainSingle();
        recipes.First().Id.Should().Be("e275d55f-a3b0-4736-a791-9a4d6290fb27");
    }

    public async Task InitializeAsync()
    {
        var recipeId = new Guid("e275d55f-a3b0-4736-a791-9a4d6290fb27"); // Використовуємо сталий ідентифікатор
        var ingredientPasta = new Ingredient(Guid.NewGuid(), "Pasta");
        var ingredientEggs = new Ingredient(Guid.NewGuid(), "Eggs");
        var ingredientCheese = new Ingredient(Guid.NewGuid(), "Cheese");
        var ingredientBacon = new Ingredient(Guid.NewGuid(), "Bacon");

        await Context.Ingredients.AddRangeAsync(ingredientPasta, ingredientEggs, ingredientCheese, ingredientBacon);
        await SaveChangesAsync();  // Зберігаємо інгредієнти в базі даних

        var recipeIngredients = new List<RecipeIngredient>
        {
            new RecipeIngredient(recipeId, RecipeIngredientsId.New(), ingredientPasta.Id, "Pasta", 200, "grams"),
            new RecipeIngredient(recipeId, RecipeIngredientsId.New(), ingredientEggs.Id, "Eggs", 3, "pieces"),
            new RecipeIngredient(recipeId, RecipeIngredientsId.New(), ingredientCheese.Id, "Cheese", 100, "grams"),
            new RecipeIngredient(recipeId, RecipeIngredientsId.New(), ingredientBacon.Id, "Bacon", 150, "grams")
        };

        var recipe = new Recipe("Spaghetti Carbonara", "Cook pasta, add eggs and cheese, mix with bacon.", TimeSpan.FromMinutes(30))
        {
            Id = recipeId,
            RecipeIngredients = recipeIngredients
        };

        await Context.Recipes.AddAsync(recipe);
        await SaveChangesAsync();  // Зберігаємо рецепт і інгредієнти в базі даних
    }



    public async Task DisposeAsync()
    {
        Context.Recipes.RemoveRange(Context.Recipes);
        await SaveChangesAsync();
    }
}
