using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int.Boards;
using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Services.App.Int.UserLevels;

namespace GamifiedToDo.Services.App.Chores;

public class ChoreService : IChoreService
{
    private readonly IChoreRepository _choreRepository;
    private readonly IUserLevelService _userLevelService;
    private readonly IBoardService _boardService;

    public ChoreService(
        IChoreRepository choreRepository, 
        IUserLevelService userLevelService,
        IBoardService boardService)
    {
        _choreRepository = choreRepository;
        _userLevelService = userLevelService;
        _boardService = boardService;
    }
    
    public Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default)
    {
        return _choreRepository.GetUserChores(userId, cancellationToken);
    }

    public Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default)
    {
        return _choreRepository.GetChoreById(choreId, userId, cancellationToken);
    }

    public async Task<Chore> AddChore(ChoreAddInput input, CancellationToken cancellationToken = default)
    {
        var chore = await _choreRepository.AddChore(input, cancellationToken).ConfigureAwait(false);
        if (input.BoardId != null)
        {
            await _boardService.AddChores(new BoardChoresInput
            {
                Id = input.BoardId,
                UserId = input.UserId,
                ChoreIds = new List<string> { chore.Id }
            }, cancellationToken).ConfigureAwait(false);
        }

        return chore;
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