using Api.Dtos;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Domain.Ingradients;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Recipes;

namespace Api.Controllers
{
    [Route("ingredients")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientsController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        // Отримання інгредієнта за ID
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)] // Не показувати в Swagger
        public async Task<ActionResult<IngredientResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var ingredientOption = await _ingredientRepository.GetById(new IngredientId(id), cancellationToken);

            if (ingredientOption == null)
            {
                return NotFound();
            }

            return Ok(IngredientResponseDto.FromDomainModel(ingredientOption));
        }

        // Отримання всіх інгредієнтів
        [HttpGet]
        public async Task<ActionResult<List<IngredientResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientRepository.GetAll(cancellationToken);

            if (ingredients == null || ingredients.Count == 0)
            {
                return NotFound();
            }

            var ingredientDtos = ingredients.Select(IngredientResponseDto.FromDomainModel).ToList();
            return Ok(ingredientDtos);
        }

        // Створення нового інгредієнта
        [HttpPost]
        public async Task<ActionResult<IngredientRequestDto>> Create([FromBody] IngredientRequestDto request, CancellationToken cancellationToken)
        {
            var ingredient = Ingredient.New(request.Name);

            await _ingredientRepository.AddAsync(ingredient, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = ingredient.Id.Value }, IngredientRequestDto.FromDomainModel(ingredient));
        }

        // Оновлення інгредієнта
        [HttpPut("{id}")]
        public async Task<ActionResult<IngredientRequestDto>> Update([FromRoute] Guid id, [FromBody] IngredientRequestDto request, CancellationToken cancellationToken)
        {
            var ingredientOption = await _ingredientRepository.GetById(new IngredientId(id), cancellationToken);

            if (ingredientOption == null)
            {
                return NotFound("Ingredient not found.");
            }

            ingredientOption.UpdateName(request.Name);
            await _ingredientRepository.Update(ingredientOption, cancellationToken);

            return Ok(IngredientRequestDto.FromDomainModel(ingredientOption));
        }

        // Видалення інгредієнта
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var ingredientOption = await _ingredientRepository.GetById(new IngredientId(id), cancellationToken);

            if (ingredientOption == null)
            {
                return NotFound("Ingredient not found.");
            }

            await _ingredientRepository.Delete(ingredientOption, cancellationToken);
            return NoContent();
        }
    }
}
