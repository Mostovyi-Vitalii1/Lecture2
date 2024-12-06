using Api.Controllers;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Recipes;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Optional;
using Xunit;

public class FavoriteRecipesControllerTests
{
    private readonly Mock<IUserRRepository> _userRepositoryMock;
    private readonly Mock<IFavoriteRecipesRepository> _favoriteRecipesRepositoryMock;
    private readonly Mock<IFavoriteRecipesQueries> _favoriteRecipesQueriesMock;
    private readonly ISender _sender;

    public FavoriteRecipesControllerTests()
    {
        _userRepositoryMock = new Mock<IUserRRepository>();
        _favoriteRecipesRepositoryMock = new Mock<IFavoriteRecipesRepository>();
        _favoriteRecipesQueriesMock = new Mock<IFavoriteRecipesQueries>();
        _sender = new Mock<ISender>().Object;
    }

    [Fact]
    public async Task GetByUserId_ReturnsFavoriteRecipes()
    {
        var userId = Guid.NewGuid();
        var favoriteRecipes = new List<FavoriteRecipes>
        {
            new FavoriteRecipes(new UserRId(userId), Guid.NewGuid())
        };

        _favoriteRecipesQueriesMock
            .Setup(x => x.GetAllByUserId(It.IsAny<UserRId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(favoriteRecipes);

        var controller = new FavoriteRecipesController(_sender, _favoriteRecipesRepositoryMock.Object, _favoriteRecipesQueriesMock.Object);
        var result = await controller.GetByUserId(userId, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<FavoriteRecipesDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task CreateFavoriteRecipe_ReturnsBadRequest_WhenRecipeAlreadyInFavorites()
    {
        var userId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        var existingFavorite = Option.Some(new FavoriteRecipes(new UserRId(userId), recipeId));

        _favoriteRecipesQueriesMock
            .Setup(x => x.GetByUserIdAndRecipeId(It.IsAny<UserRId>(), It.IsAny<RecipeId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingFavorite);

        var controller = new FavoriteRecipesController(_sender, _favoriteRecipesRepositoryMock.Object, _favoriteRecipesQueriesMock.Object);
        var result = await controller.Create(new FavoriteRecipesDto(Guid.NewGuid(), userId, recipeId), CancellationToken.None);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("This recipe is already in your favorites.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteFavoriteRecipe_ReturnsNoContent_WhenSuccessful()
    {
        var favoriteRecipeId = Guid.NewGuid();
        var favoriteRecipe = new FavoriteRecipes(new UserRId(Guid.NewGuid()), Guid.NewGuid());

        // Налаштовуємо мок для GetById, щоб він повертав Option.Some(favoriteRecipe)
        _favoriteRecipesRepositoryMock
            .Setup(x => x.GetById(It.IsAny<FavoriteRecipesId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option.Some(favoriteRecipe)); // Повертаємо Option.Some з об'єктом

        var controller = new FavoriteRecipesController(_sender, _favoriteRecipesRepositoryMock.Object, _favoriteRecipesQueriesMock.Object);
        var result = await controller.Delete(favoriteRecipeId, CancellationToken.None);

        // Перевірка, чи повертається статус NoContent (204)
        Assert.IsType<NoContentResult>(result);
    }
}
