using Application.Common.Interfaces.Repositories;
using CSharpFunctionalExtensions;
using Domain.Ingradients;
using MediatR;

namespace Application.Ingradients.Commands;
    public class CreateIngredientCommand : IRequest<Result<Ingredient, string>>
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }

    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, Result<Ingredient, string>>
    {
        private readonly IIngredientRepository _repository;

        public CreateIngredientCommandHandler(IIngredientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Ingredient, string>> Handle(CreateIngredientCommand request,
            CancellationToken cancellationToken)
        {
            // Перевірка, чи інгредієнт вже існує
            var existingIngredient = await _repository.GetByNameAsync(request.Name, cancellationToken);
            if (existingIngredient != null)
            {
                return Result.Failure<Ingredient, string>("Ingredient already exists");
            }

            // Створення нового інгредієнту
            var ingredient = Ingredient.New(request.Name);

            // Збереження інгредієнту в репозиторії
            await _repository.Add(ingredient, cancellationToken);

            return Result.Success<Ingredient, string>(ingredient); // Повертаємо успіх з новим інгредієнтом
        }
    }

