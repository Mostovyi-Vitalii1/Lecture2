using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.Recipes;
using MediatR;
using Optional.Unsafe;

public class UpdateRecipeCommand : IRequest, IRequest<Unit>
{
    public Guid RecipeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; }

    public UpdateRecipeCommand(Guid recipeId, string name, string description, int preparationTimeMinutes, List<RecipeIngredient> recipeIngredients)
    {
        RecipeId = recipeId;
        Name = name;
        Description = description;
        PreparationTimeMinutes = preparationTimeMinutes;
        RecipeIngredients = recipeIngredients;
    }
}

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Unit>
{
    private readonly IRecipeRepository _recipeRepository;

    public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipeOption = await _recipeRepository.GetById(new RecipeId(request.RecipeId), cancellationToken);

        var recipe = recipeOption.ValueOrFailure();

        recipe.UpdateDetails(request.Name, request.Description, request.PreparationTimeMinutes, request.RecipeIngredients);

        await _recipeRepository.Update(recipe, cancellationToken);

        return Unit.Value; // Завжди повертається Unit.Value для команд
    }
}