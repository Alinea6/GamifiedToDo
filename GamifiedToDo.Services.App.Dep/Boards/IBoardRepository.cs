using GamifiedToDo.Services.App.Int.Boards;

namespace GamifiedToDo.Services.App.Dep.Boards;

public interface IBoardRepository
{
    Task<Board> GetById(string boardId, string userId, CancellationToken cancellationToken = default);
    Task<Board> Add(BoardAddInput input, CancellationToken cancellationToken = default);
    Task DeleteById(string boardId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardListItem>> GetUserBoards(string userId, CancellationToken cancellationToken = default);
    Task<Board> AddChores(BoardChoresInput input, CancellationToken cancellationToken = default);
    Task<Board> RemoveChores(BoardChoresInput input, CancellationToken cancellationToken = default);
    Task<Board> AddCollaborators(BoardCollaboratorsInput input, CancellationToken cancellationToken = default);
}