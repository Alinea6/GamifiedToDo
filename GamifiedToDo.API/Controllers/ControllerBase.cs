namespace GamifiedToDo.API.Controllers;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected string? GetUserId()
    {
        return User.Claims.FirstOrDefault()?.Value;
    }
}