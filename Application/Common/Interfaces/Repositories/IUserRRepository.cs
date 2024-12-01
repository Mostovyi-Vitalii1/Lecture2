using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRRepository
{
    Task<UserR> Add(UserR user, CancellationToken cancellationToken);
    Task<UserR> Update(UserR user, CancellationToken cancellationToken);
    Task<UserR> Delete(UserR user, CancellationToken cancellationToken);
    Task<Option<UserR>> GetById(UserRId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserR>> GetAll(CancellationToken cancellationToken);
    Task<Option<UserR>> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken);
}