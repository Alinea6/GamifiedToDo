namespace GamifiedToDo.Services.App.Int.Chores;

public interface IChoreService
{
    Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default);
    Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default);
}