using GamifiedToDo.API.Models;
using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;
using Microsoft.AspNetCore.Mvc;

namespace GamifiedToDo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChoreController : ControllerBase
{
    private readonly IChoreService _choreService;

    public ChoreController(IChoreService choreService)
    {
        _choreService = choreService;
    }

    [HttpGet]
    public Task<IEnumerable<Chore>> GetUserChores(CancellationToken cancellationToken = default)
    {
        return _choreService.GetUserChores(GetUserId(), cancellationToken);
    }

    [HttpGet("{choreId}")]
    public Task<Chore> GetChoreById(string choreId, CancellationToken cancellationToken = default)
    {
        return _choreService.GetChoreById(choreId, GetUserId(), cancellationToken);
    }

    [HttpPost]
    public Task<Chore> AddChore(ChoreUpdateRequest request, CancellationToken cancellationToken = default)
    {
        return _choreService.AddChore(new ChoreAddInput
        {
            ChoreText = request.ChoreText,
            UserId = GetUserId(),
            Status = ChoreStatus.ToDo
        }, cancellationToken);
    }

    [HttpDelete("{choreId}")]
    public Task DeleteChoreById(string choreId, CancellationToken cancellationToken = default)
    {
        return _choreService.DeleteChoreById(choreId, GetUserId(), cancellationToken);
    }
}