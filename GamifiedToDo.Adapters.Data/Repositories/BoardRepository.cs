using GamifiedToDo.Services.App.Dep.Boards;
using GamifiedToDo.Services.App.Int.Boards;
using GamifiedToDo.Services.App.Int.Chores;
using Microsoft.EntityFrameworkCore;
using Board = GamifiedToDo.Services.App.Int.Boards.Board;
using User = GamifiedToDo.Services.App.Int.Users.User;

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

        return MapToBoard(board, userId);
    }

    public async Task<Board> Add(BoardAddInput input, CancellationToken cancellationToken = default)
    {
        var chores = await _context.Chores
            .Where(x => input.ChoreIds.Contains(x.Id) && x.UserId == input.UserId)
            .ToListAsync(cancellationToken);

        var users = await _context.Users
            .Where(x => input.Collaborators.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var owner = await _context.Users
            .Where(x => x.Id == input.UserId)
            .SingleAsync(cancellationToken);
        
        var board = new Models.Board
        {
            Id = Guid.NewGuid().ToString(),
            UserId = input.UserId,
            Name = input.Name,
            Chores = chores,
            Collaborators = users.ToList(),
            Owner = owner
        };

        _context.Add(board);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return MapToBoard(board, input.UserId);
    }

    public async Task DeleteById(string boardId, string userId, CancellationToken cancellationToken = default)
    {
        var board = await GetByBoardIdAndUserId(boardId, userId, cancellationToken).ConfigureAwait(false);
        
        _context.Remove(board);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<BoardListItem>> GetUserBoards(string userId, CancellationToken cancellationToken = default)
    {
        var boards = await _context.Boards
            .Include(x => x.Collaborators)
            .Include(x => x.Owner)
            .Where(b => b.UserId == userId || b.Collaborators.Any(x => x.Id == userId))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return boards.Select(x => MapToBoardListItem(x, userId, MapToUser(x.Owner)));
    }

    public async Task<Board> AddChores(BoardChoresInput input, CancellationToken cancellationToken = default)
    {
        var board = await GetByBoardIdAndUserId(input.Id, input.UserId, cancellationToken).ConfigureAwait(false);

        var chores = await _context.Chores
            .Where(x => input.ChoreIds.Contains(x.Id) && x.UserId == input.UserId)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        foreach (var chore in chores)
        {
            board.Chores.Add(chore);
        }
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToBoard(board, input.UserId);
    }

    public async Task<Board> RemoveChores(BoardChoresInput input, CancellationToken cancellationToken = default)
    {
        var board = await GetByBoardIdAndUserId(input.Id, input.UserId, cancellationToken).ConfigureAwait(false);

        var choresToRemove = board.Chores.Where(x => input.ChoreIds.Contains(x.Id)).ToList();

        foreach (var chore in choresToRemove)
        {
            board.Chores.Remove(chore);
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToBoard(board, input.UserId);
    }

    public async Task<Board> AddCollaborators(BoardCollaboratorsInput input, CancellationToken cancellationToken = default)
    {
        var board = await GetByBoardIdAndUserId(input.Id, input.UserId, cancellationToken).ConfigureAwait(false);

        var collaborators = await _context.Users
            .Where(x => input.CollaboratorIds.Contains(x.Id))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        foreach (var collaborator in collaborators)
        {
            board.Collaborators.Add(collaborator);
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToBoard(board, input.UserId);
    }

    public async Task<Board> RemoveCollaborators(BoardCollaboratorsInput input, CancellationToken cancellationToken = default)
    {
        var board = await GetByBoardIdAndUserId(input.Id, input.UserId, cancellationToken).ConfigureAwait(false);

        var collaboratorsToRemove = board.Collaborators.Where(x => input.CollaboratorIds.Contains(x.Id))
            .ToList();

        foreach (var collaborator in collaboratorsToRemove)
        {
            board.Collaborators.Remove(collaborator);
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToBoard(board, input.UserId);
    }

    private async Task<Models.Board> GetByBoardIdAndUserId(string boardId, string userId,
        CancellationToken cancellationToken = default)
    {
        var board = await _context.Boards
            .Include(x => x.Chores)
            .Include(x => x.Collaborators)
            .Include(x => x.Owner)
            .Where(b => (b.Id == boardId && b.UserId == userId) || (b.Id == boardId && b.Collaborators.Any(x => x.Id == userId)))
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (board == null)
        {
            throw new Exception($"Board with id {boardId} for user {userId} was not found");
        }

        return board;
    }

    private static Board MapToBoard(Models.Board board, string userId)
    {
        return new Board
        {
            Id = board.Id,
            Owner = MapToUser(board.Owner),
            Collaborators = board.Collaborators.Select(MapToUser),
            Name = board.Name,
            Chores = board.Chores.Select(MapToChore),
            IsOwner = board.Owner.Id == userId
        };
    }

    private static User MapToUser(Models.User user)
    {
        return new User
        {
            Id = user.Id,
            Login = user.Login
        };
    }

    private static Chore MapToChore(Models.Chore chore)
    {
        var isParsed = Enum.TryParse(chore.Status, out ChoreStatus status);
        return new Chore
        {
            Id = chore.Id,
            UserId = chore.UserId,
            Status = isParsed ? status : ChoreStatus.ToDo,
            ChoreText = chore.ChoreText,
            Difficulty = (ChoreDifficulty)chore.Difficulty,
            Category = (ChoreCategory)chore.Category
        };
    }

    private static BoardListItem MapToBoardListItem(Models.Board board, string userId, User owner)
    {
        return new BoardListItem
        {
            Id = board.Id,
            Owner = owner,
            Collaborators = board.Collaborators.Select(MapToUser),
            Name = board.Name,
            IsOwner = owner.Id == userId
        };
    }
}