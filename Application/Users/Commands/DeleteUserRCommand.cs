using Application.Common.Interfaces.Repositories;
using CSharpFunctionalExtensions;
using Domain.Users;
using MediatR;
using Optional.Unsafe;

namespace Application.Users.Commands
{
    public class DeleteUserRCommand : IRequest<Result>
    {
        public UserRId UserId { get; set; }
    }

    public class DeleteUserRCommandHandler : IRequestHandler<DeleteUserRCommand, Result>
    {
        private readonly IUserRRepository _repository;

        public DeleteUserRCommandHandler(IUserRRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteUserRCommand request, CancellationToken cancellationToken)
        {
            // Отримуємо користувача за ID
            var userOption = await _repository.GetById(request.UserId, cancellationToken);

            // Перевіряємо, чи є значення
            if (!userOption.HasValue)
            {
                return Result.Failure("User not found");
            }

            // Використовуємо ValueOrDefault для безпечного доступу до значення
            var user = userOption.ValueOrDefault();

            // Якщо користувача не знайдено (не вдалося витягти значення)
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            // Видаляємо користувача
            await _repository.Delete(user, cancellationToken);

            return Result.Success();
        }
    }
}