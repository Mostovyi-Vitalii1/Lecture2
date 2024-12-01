using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class UserRRepository : IUserRRepository, IUserRQueries
{
    private readonly ApplicationDbContext _context;
    private IUserRQueries _userRQueriesImplementation;

    public UserRRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Option<UserR>> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken)
    {
        var entity = await _context.UsersR
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName, cancellationToken);
        
        return entity == null ? Option.None<UserR>() : Option.Some(entity);
    }

    public async Task<Option<UserR>> GetById(UserRId id, CancellationToken cancellationToken)
    {
        var entity = await _context.UsersR
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<UserR>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<UserR>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.UsersR
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }



    public async Task<UserR> Add(UserR user, CancellationToken cancellationToken)
    {
        await _context.UsersR.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<UserR> Update(UserR user, CancellationToken cancellationToken)
    {
        _context.UsersR.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<UserR> Delete(UserR user, CancellationToken cancellationToken)
    {
        _context.UsersR.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }
}