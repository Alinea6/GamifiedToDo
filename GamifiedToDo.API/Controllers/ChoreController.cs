using System.Collections;
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
}