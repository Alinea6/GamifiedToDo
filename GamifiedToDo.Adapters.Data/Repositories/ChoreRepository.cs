using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data.Repositories;

public class ChoreRepository : IChoreRepository
{
    private readonly DataContext _context;

    public ChoreRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default)
    {
        var chores = await _context.Chores
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return chores.Select(MapToChore);
    }

    public async Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default)
    {
        var chore = await _context.Chores
            .Where(c => c.Id == choreId && c.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (chore == null)
        {
            throw new Exception($"Chore with id {choreId} for user {userId} was not found");
        }

        return MapToChore(chore);
    }

    private static Chore MapToChore(Models.Chore input)
    {
        var isParsed = Enum.TryParse(input.Status, out ChoreStatus status);
        return new Chore
        {
            Id = input.Id,
            UserId = input.UserId,
            Status = isParsed ? status : ChoreStatus.ToDo,
            ChoreText = input.ChoreText
        };
    }
}