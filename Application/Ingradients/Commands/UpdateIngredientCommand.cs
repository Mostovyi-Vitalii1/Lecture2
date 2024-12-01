using Application.Common.Interfaces.Repositories;
using CSharpFunctionalExtensions;
using Domain.Ingradients;
using Domain.Recipes;
using MediatR;

namespace Application.Ingradients.Commands
{
    public class UpdateIngredientCommand : IRequest<Result<Ingredient, string>>
    {
        public IngredientId IngredientId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }  // Якщо потрібно змінити кількість
        public string Unit { get; set; }      // Якщо потрібно змінити одиницю вимірювання
    }

    public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, Result<Ingredient, string>>
    {
        private readonly IIngredientRepository _repository;

        public UpdateIngredientCommandHandler(IIngredientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Ingredient, string>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            // Отримуємо інгредієнт за ID
            var ingredientOption = await _repository.GetById(request.IngredientId, cancellationToken);

            if (ingredientOption == null)
            {
                return Result.Failure<Ingredient, string>("Ingredient not found"); // Помилка, якщо інгредієнт не знайдений
            }

            // Оновлюємо деталі інгредієнта
            ingredientOption.UpdateName(request.Name);

            // Зберігаємо оновлений інгредієнт у репозиторії
            await _repository.Update(ingredientOption, cancellationToken);

            return Result.Success<Ingredient, string>(ingredientOption); // Повертаємо успіх з оновленим інгредієнтом
        }
    }
}