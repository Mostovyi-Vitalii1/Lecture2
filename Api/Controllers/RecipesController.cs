using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Recipes;
using Domain.Ingradients;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional.Unsafe;

namespace Api.Controllers
{
    [Route("recipes")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IRecipeQueries _recipeQueries;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository; // Репозиторій інгредієнтів

        public RecipesController(ISender sender, IRecipeQueries recipeQueries, IRecipeRepository recipeRepository,
            IIngredientRepository ingredientRepository)
        {
            _sender = sender;
            _recipeQueries = recipeQueries;
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository; // Ініціалізація репозиторію інгредієнтів
        }

        // Додавання нового рецепту
        [HttpPost]
        public async Task<ActionResult<RecipeDto>> Create(
            [FromBody] RecipeDto request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Повертає помилки валідації
            }

            if (request.Ingredients == null)
            {
                return BadRequest("Ingredients cannot be null.");
            }

            var ingredients = new List<RecipeIngredient>();

            foreach (var ri in request.Ingredients)
            {
                var ingredient = await _ingredientRepository.GetByNameAsync(ri.Name, cancellationToken);
                if (ingredient == null)
                {
                    ingredient = Ingredient.New(ri.Name);
                    await _ingredientRepository.AddAsync(ingredient, cancellationToken);
                }

                ingredients.Add(new RecipeIngredient(
                    Guid.NewGuid(),
                    RecipeIngredientsId.New(),
                    ingredient.Id,
                    ri.Name,
                    ri.Quantity,
                    ri.Unit
                ));
            }

            var command = new CreateRecipeCommand
            {
                Name = request.Name,
                Description = request.Description,
                PreparationTimeMinutes = request.PreparationTimeMinutes,
                Ingredients = ingredients
            };

            if (command.Ingredients == null || !command.Ingredients.Any())
            {
                return BadRequest("Ingredients are required.");
            }

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(RecipeDto.FromDomainModel(result.Value));
            }

            return BadRequest(result.Error.Message);
        }

        // Оновлення рецепту
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(
            Guid id,
            [FromBody] RecipeDto request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Ingredients == null)
            {
                return BadRequest("Ingredients cannot be null.");
            }

            var ingredients = new List<RecipeIngredient>();

            foreach (var ri in request.Ingredients)
            {
                var ingredient = await _ingredientRepository.GetByNameAsync(ri.Name, cancellationToken);
                if (ingredient == null)
                {
                    ingredient = Ingredient.New(ri.Name);
                    await _ingredientRepository.AddAsync(ingredient, cancellationToken);
                }

                ingredients.Add(new RecipeIngredient(
                    Guid.NewGuid(),
                    RecipeIngredientsId.New(),
                    ingredient.Id,
                    ri.Name,
                    ri.Quantity,
                    ri.Unit
                ));
            }

            var command = new UpdateRecipeCommand(
                id,
                request.Name,
                request.Description,
                request.PreparationTimeMinutes,
                ingredients
            );

            await _sender.Send(command, cancellationToken);

            return NoContent(); // Повертає статус 204 для успішного оновлення
        }

        // Перегляд усіх рецептів
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAll(CancellationToken cancellationToken)
        {
            var recipes = await _recipeQueries.GetAll(cancellationToken); // Передбачається, що є відповідний запит для отримання всіх рецептів
            if (recipes == null || !recipes.Any())
            {
                return NotFound("No recipes found.");
            }

            var result = recipes.Select(r => RecipeDto.FromDomainModel(r)).ToList();
            return Ok(result);
        }

        // Видалення рецепту за id
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var recipeOption = await _recipeRepository.GetById(new RecipeId(id), cancellationToken);
            var recipe = recipeOption.ValueOrFailure();

            if (recipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            // Видаляємо залежності, наприклад, зв'язки з інгредієнтами
            await _recipeRepository.Delete(recipe, cancellationToken);

            return NoContent(); // Повертаємо статус 204 для успішного видалення
        }
    }
}
