using GamifiedToDo.Services.App.Dep.UserLevels;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data.Repositories;

public class UserLevelRepository : IUserLevelRepository
{
    private readonly DataContext _context;

    public UserLevelRepository(DataContext context)
    {
        _context = context;
    }


    public async Task<int> UpdateExp(string userId, int experienceToAdd, CancellationToken cancellationToken = default)
    {
        var userLevel = await GetByUserId(userId, cancellationToken).ConfigureAwait(false);

        userLevel.Exp += experienceToAdd;
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return userLevel.Exp;
    }

    private async Task<Models.UserLevel> GetByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var userLevel = await _context.UserLevels
            .Where(uL => uL.Id == userId)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (userLevel == null)
        {
            throw new Exception($"User level with userId {userId} does not exist");
        }

        return userLevel;
    }
}