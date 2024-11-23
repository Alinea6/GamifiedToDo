using GamifiedToDo.API.Models.Chores;
using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamifiedToDo.API.Controllers;

[Authorize]
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
    public Task<Chore> AddChore(ChoreAddRequest request, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        return _choreService.AddChore(new ChoreAddInput
        {
            ChoreText = request.ChoreText,
            UserId = userId,
            Status = ChoreStatus.ToDo
        }, cancellationToken);
    }

    [HttpPut("{choreId}")]
    public Task<Chore> UpdateChoreById(
        string choreId, 
        ChoreUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return _choreService.UpdateChoreById(new ChoreUpdateInput
        {
            Id = choreId,
            UserId = GetUserId(),
            ChoreText = request.ChoreText,
            Status = request.Status
        }, cancellationToken);
    }

    [HttpDelete("{choreId}")]
    public Task DeleteChoreById(string choreId, CancellationToken cancellationToken = default)
    {
        return _choreService.DeleteChoreById(choreId, GetUserId(), cancellationToken);
    }
}