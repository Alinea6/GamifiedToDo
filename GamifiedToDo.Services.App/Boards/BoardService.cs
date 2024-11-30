using GamifiedToDo.Services.App.Dep.Boards;
using GamifiedToDo.Services.App.Int.Boards;

namespace GamifiedToDo.Services.App.Boards;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    
    public BoardService(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public Task<Board> GetById(string boardId, string userId, CancellationToken cancellationToken = default)
    {
        return _boardRepository.GetById(boardId, userId, cancellationToken);
    }

    public Task<Board> Add(BoardAddInput input, CancellationToken cancellationToken = default)
    {
        return _boardRepository.Add(input, cancellationToken);
    }
}