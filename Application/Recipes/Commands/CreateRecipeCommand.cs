using Application.Common.Interfaces.Repositories;
using MediatR;
using CSharpFunctionalExtensions;
using Domain.Recipes;

public record CreateRecipeCommand : IRequest<Result<Recipe, RecipeCreationException>>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int PreparationTimeMinutes { get; init; }
    public required List<RecipeIngredient> Ingredients { get; init; }
}
public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, Result<Recipe, RecipeCreationException>>
{
    private readonly IRecipeRepository _recipeRepository;

    public CreateRecipeCommandHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Result<Recipe, RecipeCreationException>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var recipe = Recipe.New(
                request.Name,
                request.Description,
                request.PreparationTimeMinutes,
                request.Ingredients);

            var addedRecipe = await _recipeRepository.Add(recipe, cancellationToken);
            return Result.Success<Recipe, RecipeCreationException>(addedRecipe);
        }
        catch (Exception ex)
        {
            return Result.Failure<Recipe, RecipeCreationException>(
                new RecipeCreationException($"Failed to create recipe '{request.Name}'.", ex));
        }
    }
}
