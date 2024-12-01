using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int.Chores;
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
        var chore = await GetChoreByIdAndUserId(choreId, userId, cancellationToken);

        return MapToChore(chore);
    }

    public async Task<Chore> AddChore(ChoreAddInput input, CancellationToken cancellationToken = default)
    {
        var chore = new Models.Chore
        {
            Id = Guid.NewGuid().ToString(),
            UserId = input.UserId,
            ChoreText = input.ChoreText,
            Status = input.Status.ToString(),
            Difficulty = (int)input.Difficulty,
            Category = (int)input.Category        };

        _context.Add(chore);
        await _context.SaveChangesAsync(cancellationToken);
        return MapToChore(chore);
    }

    public async Task DeleteChoreById(string choreId, string? userId, CancellationToken cancellationToken = default)
    {
        var chore = await GetChoreByIdAndUserId(choreId, userId, cancellationToken);

        _context.Remove(chore);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Chore> UpdateChoreById(ChoreUpdateInput input, CancellationToken cancellationToken = default)
    {
        var chore = await GetChoreByIdAndUserId(input.Id, input.UserId, cancellationToken);
        
        chore.ChoreText = input.ChoreText ?? chore.ChoreText;
        chore.Difficulty = input.Difficulty != null ? (int)input.Difficulty : chore.Difficulty;
        chore.Category = input.Category != null ? (int)input.Category : chore.Category;

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToChore(chore);
    }

    public async Task<Chore> UpdateStatusById(ChoreUpdateStatusInput input, CancellationToken cancellationToken = default)
    {
        var chore = await GetChoreByIdAndUserId(input.Id, input.UserId, cancellationToken);

        chore.Status = input.Status.ToString();

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
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
            ChoreText = input.ChoreText,
            Difficulty = (ChoreDifficulty)input.Difficulty,
            Category = (ChoreCategory)input.Category
        };
    }

    private async Task<Models.Chore> GetChoreByIdAndUserId(
        string choreId, 
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var chore = await _context.Chores
            .Where(c => c.Id == choreId && c.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (chore == null)
        {
            throw new Exception($"Chore with id {choreId} for user {userId} was not found");
        }

        return chore;
    }
}