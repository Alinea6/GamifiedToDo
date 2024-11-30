namespace GamifiedToDo.Services.App.Int.Boards;

public interface IBoardService
{
    Task<Board> GetById(string boardId, string userId, CancellationToken cancellationToken = default);
    Task<Board> Add(BoardAddInput input, CancellationToken cancellationToken = default);
}