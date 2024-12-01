using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IUserRQueries
{
    Task<IReadOnlyList<UserR>> GetAll(CancellationToken cancellationToken);
    Task<Option<UserR>> GetById(UserRId id, CancellationToken cancellationToken);
    Task<Option<UserR>> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken);
}