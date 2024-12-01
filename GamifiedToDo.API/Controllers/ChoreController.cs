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
    public Task<Chore> GetById(string choreId, CancellationToken cancellationToken = default)
    {
        return _choreService.GetChoreById(choreId, GetUserId(), cancellationToken);
    }

    [HttpPost]
    public Task<Chore> Add(ChoreAddRequest request, CancellationToken cancellationToken = default)
    {
        return _choreService.AddChore(new ChoreAddInput
        {
            ChoreText = request.ChoreText,
            UserId = GetUserId(),
            Status = ChoreStatus.ToDo,
            Difficulty = request.Difficulty,
            Category = request.Category
        }, cancellationToken);
    }

    [HttpPut("{choreId}")]
    public Task<Chore> UpdateById(
        string choreId, 
        ChoreUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return _choreService.UpdateChoreById(new ChoreUpdateInput
        {
            Id = choreId,
            UserId = GetUserId(),
            ChoreText = request.ChoreText,
            Difficulty = request.Difficulty,
            Category = request.Category
        }, cancellationToken);
    }

    [HttpDelete("{choreId}")]
    public Task DeleteById(string choreId, CancellationToken cancellationToken = default)
    {
        return _choreService.DeleteChoreById(choreId, GetUserId(), cancellationToken);
    }

    [HttpPut("{choreId}/status")]
    public Task<Chore> UpdateStatusById(
        string choreId,
        ChoreUpdateStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        return _choreService.UpdateStatusById(new ChoreUpdateStatusInput
        {
            Id = choreId,
            UserId = GetUserId(),
            Status = request.Status
        }, cancellationToken);
    }
}