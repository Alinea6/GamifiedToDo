using GamifiedToDo.Services.App.Dep.Boards;
using GamifiedToDo.Services.App.Int.Boards;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly DataContext _context;

    public BoardRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Board> GetById(string boardId, string userId, CancellationToken cancellationToken = default)
    {
        var board = await GetByBoardIdAndUserId(boardId, userId, cancellationToken).ConfigureAwait(false);

        return MapToBoard(board);
    }

    public async Task<Board> Add(BoardAddInput input, CancellationToken cancellationToken = default)
    {
        var board = new Models.Board
        {
            Id = Guid.NewGuid().ToString(),
            UserId = input.UserId,
            Name = input.Name,
            ChoreIds = input.ChoreIds.ToArray(),
            Collaborators = input.Collaborators.ToArray()
        };

        _context.Add(board);
        await _context.SaveChangesAsync(cancellationToken);
        return MapToBoard(board);
    }

    private async Task<Models.Board> GetByBoardIdAndUserId(string boardId, string userId,
        CancellationToken cancellationToken = default)
    {
        //TODO: Fix Collaborators and tasks to be able to look for boards by collaborators id, probably need to add relations
        var board = await _context.Boards
            .Where(b => b.Id == boardId && b.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (board == null)
        {
            throw new Exception($"Board with id {boardId} for user {userId} was not found");
        }

        return board;
    }

    private static Board MapToBoard(Models.Board board)
    {
        return new Board
        {
            Id = board.Id,
            UserId = board.UserId,
            Collaborators = board.Collaborators,
            Name = board.Name,
            ChoreIds = board.ChoreIds
        };
    }
}