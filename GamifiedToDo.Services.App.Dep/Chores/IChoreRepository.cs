using GamifiedToDo.Services.App.Int;

namespace GamifiedToDo.Services.App.Dep.Chores;

public interface IChoreRepository
{
    Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default);
    Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default);
}