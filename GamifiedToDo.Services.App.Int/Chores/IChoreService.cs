namespace GamifiedToDo.Services.App.Int.Chores;

public interface IChoreService
{
    Task<IEnumerable<Chore>> GetUserChores(string? userId, CancellationToken cancellationToken = default);
    Task<Chore> GetChoreById(string choreId, string? userId, CancellationToken cancellationToken = default);
    Task<Chore> AddChore(ChoreAddInput input, CancellationToken cancellationToken = default);
    Task DeleteChoreById(string choreId, string? userId, CancellationToken cancellationToken = default);
    Task<Chore> UpdateChoreById(ChoreUpdateInput input, CancellationToken cancellationToken = default);
    Task<Chore> UpdateStatusById(ChoreUpdateStatusInput input, CancellationToken cancellationToken = default);
}