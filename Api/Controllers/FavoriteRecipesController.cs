using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Recipes;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace Api.Controllers
{
    [Route("favorites")]
    [ApiController]
    public class FavoriteRecipesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IFavoriteRecipesRepository _favoriteRecipesRepository;
        private readonly IFavoriteRecipesQueries _favoriteRecipesQueries;

        public FavoriteRecipesController(ISender sender, IFavoriteRecipesRepository favoriteRecipesRepository,
            IFavoriteRecipesQueries favoriteRecipesQueries)
        {
            _sender = sender;
            _favoriteRecipesRepository = favoriteRecipesRepository;
            _favoriteRecipesQueries = favoriteRecipesQueries;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteRecipesDto>>> GetByUserId(
            Guid userId, 
            CancellationToken cancellationToken)
        {
            // Отримання улюблених рецептів за ідентифікатором користувача
            var favoriteRecipes = await _favoriteRecipesQueries.GetAllByUserId(new UserRId(userId), cancellationToken);

            if (!favoriteRecipes.Any())
            {
                return NotFound("No favorite recipes found for the user.");
            }

            // Мапінг доменних моделей у DTO
            var favoriteRecipesDto = favoriteRecipes
                .Select(FavoriteRecipesDto.FromDomainModel)
                .ToList();

            return Ok(favoriteRecipesDto);
        }
        // Створення улюбленого рецепту
        [HttpPost]
        public async Task<ActionResult<FavoriteRecipesDto>> Create(
            [FromBody] FavoriteRecipesDto request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Повертає помилки валідації
            }

            // Перевірка на наявність улюбленого рецепту для користувача та рецепту
            var existingFavorite = await _favoriteRecipesQueries.GetByUserIdAndRecipeId(
                new UserRId(request.UserId),
                new RecipeId(request.RecipeId),
                cancellationToken);

            if (existingFavorite.HasValue)
            {
                return BadRequest("This recipe is already in your favorites.");
            }

            // Створення нового запису в таблиці улюблених рецептів
            var favoriteRecipe = new FavoriteRecipes(
                new UserRId(request.UserId),
                new RecipeId(request.RecipeId)
                );


            await _favoriteRecipesRepository.Add(favoriteRecipe, cancellationToken);

            return Ok(FavoriteRecipesDto.FromDomainModel(favoriteRecipe));
        }

// Видалення улюбленого рецепту
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(
            Guid id, 
            CancellationToken cancellationToken)
        {
            // Отримуємо улюблений рецепт по ID
            var favoriteRecipeOption = await _favoriteRecipesRepository.GetById(
                new FavoriteRecipesId(id), cancellationToken);

            // Перевіряємо чи є результат
            if (!favoriteRecipeOption.HasValue)
            {
                return NotFound("Favorite recipe not found.");
            }

            // Отримуємо значення з Option
            var favoriteRecipe = favoriteRecipeOption.ValueOr((FavoriteRecipes)null); // Можна додати дефолтне значення, якщо потрібно

            if (favoriteRecipe == null)
            {
                return NotFound("Favorite recipe not found.");
            }

            // Якщо результат є, то видаляємо його
            await _favoriteRecipesRepository.Delete(favoriteRecipe, cancellationToken);

            return NoContent(); // Повертаємо статус 204 для успішного видалення
        }


    }
}