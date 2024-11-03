using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;

namespace GamifiedToDo.Services.App.Chores;

public class ChoreService : IChoreService
{
    private readonly IChoreRepository _choreRepository;

    public ChoreService(IChoreRepository choreRepository)
    {
        _choreRepository = choreRepository;
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
}