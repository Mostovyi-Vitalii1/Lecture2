using Application.Common.Interfaces.Repositories;
using CSharpFunctionalExtensions;
using Domain.Users;
using MediatR;
using Optional.Unsafe;

public class UpdateUserRCommand : IRequest<Result<UserR, string>>
{
    public UserRId UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    // Інші поля, якщо необхідно
}

public class UpdateUserRCommandHandler : IRequestHandler<UpdateUserRCommand, Result<UserR, string>>
{
    private readonly IUserRRepository _repository;

    public UpdateUserRCommandHandler(IUserRRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserR, string>> Handle(UpdateUserRCommand request, CancellationToken cancellationToken)
    {
        // Отримуємо користувача за ID
        var userOption = await _repository.GetById(request.UserId, cancellationToken);

        if (!userOption.HasValue)
        {
            return Result.Failure<UserR, string>("User not found"); // Помилка, якщо користувач не знайдений
        }

        var user = userOption.ValueOrDefault(); // Використовуємо ValueOrDefault()

        if (user == null)
        {
            return Result.Failure<UserR, string>("User is null"); // Якщо користувач null
        }

        // Оновлюємо дані користувача
        user.UpdateDetails(request.FirstName, request.LastName);  // Викликаємо метод UpdateDetails

        // Оновлюємо користувача в репозиторії
        await _repository.Update(user, cancellationToken);

        return Result.Success<UserR, string>(user); // Повертаємо успіх з оновленим користувачем
    }
}
