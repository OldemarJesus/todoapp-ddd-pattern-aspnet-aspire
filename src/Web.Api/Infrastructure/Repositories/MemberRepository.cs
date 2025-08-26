using Microsoft.EntityFrameworkCore;
using Web.Api.Domains;

namespace Web.Api.Infrastructure.Repositories;

public sealed class MemberRepository
{
    private readonly TodoDbContext _dbContext;

    public MemberRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Member?> GetByIdAsync(MemberId id, CancellationToken cancellationToken)
    {
        return await _dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Members
            .AsNoTracking()
            .AnyAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<Member?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }
}
