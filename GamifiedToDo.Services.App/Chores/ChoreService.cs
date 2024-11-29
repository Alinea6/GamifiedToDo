using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Services.App.Int.UserLevels;

namespace GamifiedToDo.Services.App.Chores;

public class ChoreService : IChoreService
{
    private readonly IChoreRepository _choreRepository;
    private readonly IUserLevelService _userLevelService;

    public ChoreService(IChoreRepository choreRepository, IUserLevelService userLevelService)
    {
        _choreRepository = choreRepository;
        _userLevelService = userLevelService;
    }
    
    public Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default)
    {
        return _choreRepository.GetUserChores(userId, cancellationToken);
    }

    public Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default)
    {
        return _choreRepository.GetChoreById(choreId, userId, cancellationToken);
    }

    public Task<Chore> AddChore(ChoreAddInput input, CancellationToken cancellationToken = default)
    {
        return _choreRepository.AddChore(input, cancellationToken);
    }

    public Task DeleteChoreById(string choreId, string? userId, CancellationToken cancellationToken = default)
    {
        return _choreRepository.DeleteChoreById(choreId, userId, cancellationToken);
    }

    public Task<Chore> UpdateChoreById(ChoreUpdateInput input, CancellationToken cancellationToken = default)
    {
        return _choreRepository.UpdateChoreById(input, cancellationToken);
    }

    public async Task<Chore> UpdateStatusById(ChoreUpdateStatusInput input, CancellationToken cancellationToken = default)
    {
        var chore = await _choreRepository.UpdateStatusById(input, cancellationToken).ConfigureAwait(false);
        await _userLevelService.UpdateExp(input.UserId, (int)chore.Difficulty, cancellationToken).ConfigureAwait(false);

        return chore;
    }
}