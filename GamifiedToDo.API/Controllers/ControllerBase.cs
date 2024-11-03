namespace GamifiedToDo.API.Controllers;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected string? GetUserId()
    {
        var temporaryId = "490987db-6081-42ba-abf6-f09b0bad90b3";
        return temporaryId;
        //TODO: Uncomment after setting up users
        //return User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
    }
}