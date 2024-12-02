using GamifiedToDo.API.Models.Boards;
using GamifiedToDo.Services.App.Int.Boards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamifiedToDo.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet("{boardId}")]
    public Task<Board> GetById(string boardId, CancellationToken cancellationToken = default)
    {
        return _boardService.GetById(boardId, GetUserId(), cancellationToken);
    }

    [HttpPost]
    public Task<Board> Add(BoardAddRequest request, CancellationToken cancellationToken = default)
    {
        return _boardService.Add(
            new BoardAddInput
            {
                UserId = GetUserId(),
                Name = request.Name,
                ChoreIds = request.ChoreIds,
                Collaborators = request.Collaborators
            }, cancellationToken);
    }

    [HttpDelete("{boardId}")]
    public Task DeleteById(string boardId, CancellationToken cancellationToken = default)
    {
        return _boardService.DeleteById(boardId, GetUserId(), cancellationToken);
    }

    [HttpGet]
    public Task<IEnumerable<BoardListItem>> GetUserBoards(CancellationToken cancellationToken = default)
    {
        return _boardService.GetUserBoards(GetUserId(), cancellationToken);
    }

    [HttpPost("{boardId}/chores/add")]
    public Task<Board> AddChores(string boardId, BoardChoresRequest request,
        CancellationToken cancellationToken = default)
    {
        return _boardService.AddChores(new BoardChoresInput
        {
            Id = boardId,
            UserId = GetUserId(),
            ChoreIds = request.ChoreIds
        }, cancellationToken);
    }

    [HttpPost("{boardId}/chores/remove")]
    public Task<Board> RemoveChores(string boardId, BoardChoresRequest request,
        CancellationToken cancellationToken = default)
    {
        return _boardService.RemoveChores(new BoardChoresInput
        {
            Id = boardId,
            UserId = GetUserId(),
            ChoreIds = request.ChoreIds
        }, cancellationToken);
    }
}