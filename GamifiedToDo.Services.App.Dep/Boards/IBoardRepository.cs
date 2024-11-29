using GamifiedToDo.Services.App.Int.Boards;

namespace GamifiedToDo.Services.App.Dep.Boards;

public interface IBoardRepository
{
    Task<Board> GetById(string boardId, string userId, CancellationToken cancellationToken = default);
}