using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;
using CSharpFunctionalExtensions;
using Optional;

public record CreateUserRCommand : IRequest<Result<UserR, UserRException>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}

public class CreateUserRCommandHandler : IRequestHandler<CreateUserRCommand, Result<UserR, UserRException>>
{
    private readonly IUserRRepository _userRRepository;

    public CreateUserRCommandHandler(IUserRRepository userRRepository)
    {
        _userRRepository = userRRepository;
    }

    public async Task<Result<UserR, UserRException>> Handle(CreateUserRCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRRepository.GetByFullName(request.FirstName, request.LastName, cancellationToken);

        return await existingUser.Match<Task<Result<UserR, UserRException>>>(
            some: u => Task.FromResult(Result.Failure<UserR, UserRException>(
                new UserAlreadyExistsException(u.Id))),
            none: async () => await CreateEntity(request.FirstName, request.LastName, cancellationToken));
    }

    private async Task<Result<UserR, UserRException>> CreateEntity(string firstName, string lastName, CancellationToken cancellationToken)
    {
        try
        {
            var user = UserR.New(UserRId.New(), firstName, lastName);
            var addedUser = await _userRRepository.Add(user, cancellationToken);
            return Result.Success<UserR, UserRException>(addedUser);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserR, UserRException>(new UserUnknownException(UserRId.Empty(), ex));
        }
    }
}