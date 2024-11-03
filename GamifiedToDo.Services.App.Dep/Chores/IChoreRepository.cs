using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;

namespace GamifiedToDo.Services.App.Dep.Chores;

public interface IChoreRepository
{
    Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default);
    Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default);
    Task<Chore> AddChore(ChoreAddInput input, CancellationToken cancellationToken = default);
    Task DeleteChoreById(string choreId, string? userId, CancellationToken cancellationToken = default);
}