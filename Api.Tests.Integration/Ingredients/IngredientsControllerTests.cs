using System.Net.Http.Json;
using Api.Dtos;
using Domain.Ingradients;
using Domain.Recipes;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Ingredients;

public class IngredientsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Ingredient _defaultIngredient;

    public IngredientsControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _defaultIngredient = IngredientsData.DefaultIngredient;
    }

    [Fact]
    public async Task ShouldCreateIngredient()
    {
        // Arrange
        var ingredientName = "Salt";
        var request = new IngredientRequestDto(
            name: ingredientName,
            quantity: 1,
            unit: "f");

        // Act
        var response = await Client.PostAsJsonAsync("ingredients", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseIngredient = await response.ToResponseModel<IngredientResponseDto>();
        var ingredientId = new IngredientId(responseIngredient.Id);

        var dbIngredient = await Context.Ingredients.FirstAsync(x => x.Id == ingredientId);
        dbIngredient.Name.Should().Be(ingredientName);
    }

    [Fact]
    public async Task ShouldUpdateIngredient()
    {
        // Arrange
        var newName = "Black Pepper";
        var request = new IngredientRequestDto(
            name: newName,
            quantity: 1,
            unit: "f");

        // Act
        var response = await Client.PutAsJsonAsync($"ingredients/{_defaultIngredient.Id.Value}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbIngredient = await Context.Ingredients.FirstAsync(x => x.Id == _defaultIngredient.Id);
        dbIngredient.Name.Should().Be(newName);
    }

    [Fact]
    public async Task ShouldDeleteIngredient()
    {
        // Act
        var response = await Client.DeleteAsync($"ingredients/{_defaultIngredient.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbIngredient = await Context.Ingredients.FirstOrDefaultAsync(x => x.Id == _defaultIngredient.Id);
        dbIngredient.Should().BeNull();
    }

    [Fact]
    public async Task ShouldGetAllIngredients()
    {
        // Act
        var response = await Client.GetAsync("ingredients");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var ingredients = await response.ToResponseModel<List<IngredientResponseDto>>();
        ingredients.Should().ContainSingle();
        ingredients.First().Id.Should().Be(_defaultIngredient.Id.Value);
    }

    public async Task InitializeAsync()
    {
        await Context.Ingredients.AddAsync(_defaultIngredient);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Ingredients.RemoveRange(Context.Ingredients);
        await SaveChangesAsync();
    }
}
