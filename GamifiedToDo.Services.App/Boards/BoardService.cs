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

    public Task DeleteById(string boardId, string userId, CancellationToken cancellationToken = default)
    {
        return _boardRepository.DeleteById(boardId, userId, cancellationToken);
    }

    public Task<IEnumerable<BoardListItem>> GetUserBoards(string userId, CancellationToken cancellationToken = default)
    {
        return _boardRepository.GetUserBoards(userId, cancellationToken);
    }

    public Task<Board> AddChores(BoardChoresInput input, CancellationToken cancellationToken = default)
    {
        return _boardRepository.AddChores(input, cancellationToken);
    }

    public Task<Board> RemoveChores(BoardChoresInput input, CancellationToken cancellationToken = default)
    {
        return _boardRepository.RemoveChores(input, cancellationToken);
    }

    public Task<Board> AddCollaborators(BoardCollaboratorsInput input, CancellationToken cancellationToken = default)
    {
        return _boardRepository.AddCollaborators(input, cancellationToken);
    }

    public Task<Board> RemoveCollaborators(BoardCollaboratorsInput input, CancellationToken cancellationToken = default)
    {
        return _boardRepository.RemoveCollaborators(input, cancellationToken);
    }
}